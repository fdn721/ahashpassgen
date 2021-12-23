using System;
using System.Reactive;
using AHashPassGen.Properties;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AHashPassGen.ViewModels
{
    public class AboutViewModel : ReactiveObject
    {
        public event Action? CloseEvent;
        
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable MemberCanBePrivate.Global
        public ReactiveCommand<Unit, Unit> CloseCommand { get; }

        [Reactive] public string Version { get; set; }
        [Reactive] public string CopyRight { get; set; }
        
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore MemberCanBePrivate.Global

        public AboutViewModel()
        {
            CloseCommand = ReactiveCommand.Create( CloseHandler );

            Version = I18n.Version + " " + GetType().Assembly.GetName().Version;
            CopyRight = "Dmitriy Fisenko © 2021";
        }
        
        private void CloseHandler()
        {
            CloseEvent?.Invoke();
        }

    }
}