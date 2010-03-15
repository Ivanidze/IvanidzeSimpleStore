using System;
using System.Collections.Generic;
using System.Linq;

using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Infrasturcture
{
    public class Presenters
    {
        /// <summary>
        /// Показать презентер с заданым именем и набором параметров
        /// </summary>
        /// <param name="name">имя презентера</param>
        /// <param name="args">набор параметров</param>
        public static void Show(string name, params object[] args)
        {
            var instance = CreateInstance(name, args);
            instance.Show();
        }
        /// <summary>
        /// Отобразить презентер с заданым именем в формате диалога и вернуть результат
        /// </summary>
        /// <typeparam name="T">тип результата</typeparam>
        /// <param name="name">имя презентера</param>
        /// <param name="args">набор параметров</param>
        /// <returns>результат выполнения диалога</returns>
        public static T ShowDialog<T>(string name , params object[] args)
        {
            var instance = CreateInstance(name, args);
            instance.ShowDialog();
            return (T) instance.Result;
        }
        /// <summary>
        /// Инстанциировать презентер
        /// </summary>
        /// <param name="name">имя презентера</param>
        /// <param name="args">набор параметров</param>
        /// <returns>контракт презентера</returns>
        private static IPresenter CreateInstance(string name, object[] args)
        {
            //ищем в загруженых сборках презентер с именем Presenters.{name}.Presenter
            var assemblie = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            where assembly.GetName().ToString() == "Presentation"
                            select assembly;


            var type = assemblie.First().GetType("Presenters." + name + ".Presenter");
            if (type ==null)
                throw new InvalidOperationException("Не могу найти презентер: "+name);
            //ищем в загруженых контейнерах такой тип
            var instance =  (IPresenter) Activator.CreateInstance(type);

            WireEvents(instance);
            WireButtons(instance);
            WireListBoxesDoubleClick(instance);
            if (args!=null&& args.Length>0)
            {
                var init = type.GetMethod("Initialize");
                if (init == null)
                { 
                    throw new InvalidOperationException("У презентера нет метода Initialize");
                }
                init.Invoke(instance, args);
            }


            return instance;

        }

        private static void WireListBoxesDoubleClick(IPresenter presenter)
        {
            var presenterType = presenter.GetType();
            var methodsAndListBoxes = from method in GetActionMethods(presenterType)
                                      where method.Name.EndsWith("Choosen")
                                      where method.GetParameters().Length == 1
                                      let elementName = method.Name.Substring(2, method.Name.Length - 2 - 7)
                                      let matchingListBox =
                                          LogicalTreeHelper.FindLogicalNode(presenter.View, elementName) as ListBox
                                      where matchingListBox != null
                                      select new {method, matchingListBox};
            foreach (var methodAndEvent in methodsAndListBoxes)
            {
                var paremeterType = methodAndEvent.method.GetParameters()[0].ParameterType;
                var action = Delegate.CreateDelegate(typeof (Action<>).MakeGenericType(paremeterType), presenter,
                                                     methodAndEvent.method);
                methodAndEvent.matchingListBox.MouseDoubleClick += (sender, args) =>
                   {
                       var item =
                           ((ListBox) sender).SelectedItem;
                       if (item == null)
                           return;
                       action.DynamicInvoke(item);
                   };
            }
        }

        private static void WireButtons(IPresenter presenter)
        {
            var presenterType = presenter.GetType();
            var methodsAndButtons = from method in GetParameterlessActionMethods(presenterType)
                                    let elementName = method.Name.Substring(2)
                                    let matchingControl =
                                        LogicalTreeHelper.FindLogicalNode(presenter.View, elementName) as Button
                                    let fact = presenterType.GetProperty("Can" + elementName)
                                    where matchingControl != null
                                    select new {method, fact, button = matchingControl};
            foreach (var matching in methodsAndButtons)
            {
                var action = (Action) Delegate.CreateDelegate(typeof (Action), presenter, matching.method);
                Fact fact = null;
                if (matching.fact != null)
                    fact = (Fact) matching.fact.GetValue(presenter, null);
                matching.button.Command = new DelegatingCommand(action, fact);
            }
        }

        private static void WireEvents(IPresenter presenter)
        {
            var viewType = presenter.View.GetType();
            var presenterType = presenter.GetType();
            var methodsAndEvents = from method in GetParameterlessActionMethods(presenterType)
                                   let mathcingEvent = viewType.GetEvent(method.Name.Substring(2))
                                   where mathcingEvent != null
                                   where mathcingEvent.EventHandlerType == typeof (RoutedEventHandler)
                                   select new {method, mathcingEvent};
            foreach (var methodAndEvent in methodsAndEvents)
            {
                var action = (Action)Delegate.CreateDelegate(typeof(Action), presenter, methodAndEvent.method);
                var handler = (RoutedEventHandler) ((sender, args) => action());
                methodAndEvent.mathcingEvent.AddEventHandler(presenter.View,handler);

            }
        }

        private static IEnumerable<MethodInfo> GetActionMethods(Type type)
        {
            return from method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                   where method.Name.StartsWith("On")
                   select method;
        }

        private static IEnumerable<MethodInfo> GetParameterlessActionMethods(Type type)
        {
            return from method in GetActionMethods(type)
                   where method.GetParameters().Length == 0
                   select method;
        }
    }
}
