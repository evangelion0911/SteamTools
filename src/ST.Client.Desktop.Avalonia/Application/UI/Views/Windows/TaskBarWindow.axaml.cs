using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System.Application.Services;
using System.Application.UI.ViewModels;

namespace System.Application.UI.Views.Windows
{
    public partial class TaskBarWindow : Window
    {
        public TaskBarWindow()
        {
            InitializeComponent();

            Topmost = true;
            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaTitleBarHeightHint = -1;
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
            ShowInTaskbar = false;
            SizeToContent = Avalonia.Controls.SizeToContent.Height;
            TransparencyLevelHint = WindowTransparencyLevel.AcrylicBlur;
            CanResize = false;

            this.Opened += Window_Opened;
            this.LostFocus += Window_LostFocus;
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void Window_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (!this.IsPointerOver)
            {
                if (this.DataContext is TaskBarWindowViewModel vm)
                {
                    vm.IsVisible = false;
                    this.Hide();
                }
            }
        }

        private void Window_Opened(object? sender, EventArgs e)
        {
            if (this.DataContext is TaskBarWindowViewModel vm)
            {
                //if (vm.SizePosition.X > 0 && vm.SizePosition.Y > 0)
                //{
                //    this.Position = new PixelPoint(vm.SizePosition.X, vm.SizePosition.Y - (int)this.Height);
                //}
                var workingAreaWidth = this.Screens.Primary.WorkingArea.Width;
                vm.WhenAnyValue(x => x.SizePosition.X, x => x.SizePosition.Y)
                    .Subscribe(x =>
                    {
                        if ((x.Item1 + (int)this.Width + 30) > workingAreaWidth)
                        {
                            this.Position = new PixelPoint(x.Item1 - (int)this.Width, x.Item2 - (int)this.Height);
                        }
                        else
                        {
                            this.Position = new PixelPoint(x.Item1, x.Item2 - (int)this.Height);
                        }
                    });
            }
#if Windows
            DI.Get<ISystemWindowApiService>().SetActiveWindow(new Models.HandleWindow { Handle = this.PlatformImpl.Handle.Handle });
#endif

        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}