
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Windows.Threading;
namespace Infrasturcture
{
    /// <summary>
    /// Публикатор событий
    /// </summary>
    public class EventPublisher
    {
        private readonly static IDictionary<Type,List<Action<object >>> Subscribers = new Dictionary<Type, List<Action<object>>>();
        private static readonly IDictionary<IPresenter, ICollection<object>> _preparedEvents
            = new Dictionary<IPresenter, ICollection<object>>();

        /// <summary>
        /// Опубликовать событие 
        /// </summary>
        /// <typeparam name="T">тип события</typeparam>
        /// <param name="eventToPublish">событие для публикации</param>
        /// <param name="publishedBy">каким  презентером опубликовано</param>
        public static void Publish<T>(T eventToPublish, IPresenter publishedBy)
        {
            InternalPublish(eventToPublish, typeof (T), publishedBy);
        }

        private static void InternalPublish(object eventToPublish, Type eventType, IPresenter publishedBy)
        {
            List<Action<object>> actions;
            lock (Subscribers)
            {
                if (Subscribers.TryGetValue(eventType,out actions)==false)
                    return;
            }
            foreach (var action in actions)
            {
                if (action.Target == publishedBy)
                    continue;
                action(eventToPublish);
            }
        }
        /// <summary>
        /// Добавить событие для публикации
        /// </summary>
        /// <typeparam name="T">тип события</typeparam>
        /// <param name="eventToPublish"> событие для публикации</param>
        /// <param name="publishedBy">кем опубликовано</param>
        public static void Enlist<T>(T eventToPublish, IPresenter publishedBy)
        {
            lock (_preparedEvents)
            {
                ICollection<object> eventsToPublish;
                if (!_preparedEvents.TryGetValue(publishedBy, out eventsToPublish))
                {
                    eventsToPublish = new List<object> {eventToPublish};
                    _preparedEvents[publishedBy] = eventsToPublish;
                }
                else
                {
                    eventsToPublish.Add(eventToPublish);
                }
            }
        }
        /// <summary>
        /// Выполнить все события выбранного презентера
        /// </summary>
        /// <param name="publisher"></param>
        public static void RaiseEvents(IPresenter publisher)
        {
            lock (_preparedEvents)
            {
                ICollection<object> eventsToPublish;
                if (!_preparedEvents.TryGetValue(publisher,out eventsToPublish))
                {
                    return;
                }
                foreach (var eventToPublish in eventsToPublish)
                {
                    InternalPublish(eventToPublish,eventToPublish.GetType(),publisher);
                }
                eventsToPublish.Clear();
            }
        }
        /// <summary>
        /// Зарегистрировать действие для презентера
        /// </summary>
        /// <typeparam name="T">тип дейтсвия</typeparam>
        /// <param name="action">действие</param>
        public static void Register<T>(Action<T>action)
        {
            Debug.Assert(action.Target is IPresenter);
            lock (Subscribers)
            {
                List<Action<object>> value;
                if (Subscribers.TryGetValue(typeof(T),out  value)==false)
                {
                    Subscribers[typeof (T)] = value = new List<Action<object>>();
                }
                var dispatcher = Dispatcher.CurrentDispatcher;
                Debug.Assert(dispatcher!=null);
                Action<object> item = o =>
                                          {
                                              if (dispatcher != Dispatcher.CurrentDispatcher)
                                              {
                                                  dispatcher.Invoke((Action) delegate { action((T) o); });
                                              }
                                              else
                                              {
                                                  action((T) o);
                                              }
                                          };
                    ((IPresenter) action.Target).Disposed += () => Unregister<T>(item);
                    value.Add(item);
                }
            }

        private static void Unregister<T>(Action<object> action)
        {
           lock(Subscribers)
           {
               List<Action<object>> value;
               if (Subscribers.TryGetValue(typeof(T),out value)==false)
                   return;
               value.Remove(action);
           }
        }
    }
    
}
