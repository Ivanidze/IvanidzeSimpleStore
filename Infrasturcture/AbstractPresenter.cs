using System;
using System.Windows;
namespace Infrasturcture
{
    public abstract class AbstractPresenter<TModel,TView>:IDisposable,IPresenter 
        where TView:Window, new()
    {
        protected AbstractPresenter()
        {
            View = new TView();
            View.Closed += (sender, args) => Dispose();
        }

        /// <summary>
        /// приватная ссылка на модель презентера
        /// </summary>
        private TModel model;
        /// <summary>
        /// Модель с которой работает презентер
        /// </summary>
        protected TModel Model
        {
            get { return model; }
            set
            {
                model = value;
                View.DataContext = model;
            }
        }

        DependencyObject IPresenter.View{get { return View; }} 
        /// <summary>
        /// Отображение с которой работает презентер
        /// </summary>
        protected TView View { get; set; }
        /// <summary>
        /// Разрушить презентер
        /// </summary>
        public void Dispose()
        {
            View.Close();
            Disposed();
        }
        /// <summary>
        /// При срабатывании эксепшена вывести диалог с ним
        /// </summary>
        /// <param name="exception"></param>
        public void OnException(Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
        /// <summary>
        /// Отобразить вью
        /// </summary>
        public void Show()
        {
            View.Show();
        }

        public object Result { get; protected set; }

        public event Action Disposed = delegate { };
        /// <summary>
        /// Отобразить вью в виде диалога
        /// </summary>
        public void ShowDialog()
        {
            View.ShowDialog();
        }
    }
}
