using System;
using System.Windows;
using System.ComponentModel;
namespace Infrasturcture
{
    /// <summary>
    /// Контракт для презентера
    /// </summary>
    public interface IPresenter:ISupportExceptionRecovery
    {
        /// <summary>
        /// ссылка на вьюху
        /// </summary>
        DependencyObject View { get;}
        /// <summary>
        /// отобразить вьюху
        /// </summary>
        void Show();
        /// <summary>
        /// результат выполнения
        /// </summary>
        object Result { get;}
        /// <summary>
        /// дейстивие при разрушении презентера
        /// </summary>
        event Action Disposed;
        /// <summary>
        /// отобразить вьюху в режиме диалога
        /// </summary>
        void ShowDialog();
    }
   
}
