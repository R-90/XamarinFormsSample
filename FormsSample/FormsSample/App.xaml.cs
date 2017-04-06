using System;
using Urho;
using Urho.Forms;
using Xamarin.Forms;

namespace FormsSample
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            MainPage = new NavigationPage(new StartPage { });
        }
    }

    public class StartPage : ContentPage
    {
        public StartPage()
        {
            Title = "Vision Companion Demo";
            var b1 = new Button { Text = "Launch Forms Sample" };
            var b2 = new Button { Text = "Launch Joystick Sample" };

            b1.Clicked += (sender, e) => Navigation.PushAsync(new UrhoPage());
            b2.Clicked += (sender, e) => Navigation.PushAsync(new DemoPage());    
            Content = new StackLayout { Children = { b1, b2 }, VerticalOptions = LayoutOptions.Center };
        }
    }

    public class DemoPage : ContentPage
    {
        UrhoSurface urhoSurface;
        Joystick urhoApp;
        //Charts urhoApp;
        public DemoPage()
        {
            urhoSurface = new UrhoSurface();
            urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;

            Title = "Joystick";
            Content = new StackLayout
            {
                Padding = new Thickness(12, 12, 12, 40),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    urhoSurface,
                }
            };
        }

        protected override void OnDisappearing()
        {
            UrhoSurface.OnDestroy();
            base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            StartUrhoApp();
        }

        async void StartUrhoApp()
        {
            //urhoApp = await urhoSurface.Show<Charts>(new ApplicationOptions(assetsFolder: null) { Orientation = ApplicationOptions.OrientationType.LandscapeAndPortrait });
            urhoApp = await urhoSurface.Show<Joystick>(new ApplicationOptions(assetsFolder: null) { Orientation = ApplicationOptions.OrientationType.LandscapeAndPortrait });
        }
    }

    public class UrhoPage : ContentPage
    {
        UrhoSurface urhoSurface;
        Charts urhoApp;
        Slider selectedBarSlider;

        public UrhoPage()
        {
            var restartBtn = new Button { Text = "Restart" };
            restartBtn.Clicked += (sender, e) => StartUrhoApp();

            urhoSurface = new UrhoSurface();
            urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;

            Slider rotationSlider = new Slider(0, 500, 250);
            rotationSlider.ValueChanged += (s, e) => urhoApp?.Rotate((float)(e.NewValue - e.OldValue));

            selectedBarSlider = new Slider(0, 5, 2.5);
            selectedBarSlider.ValueChanged += OnValuesSliderValueChanged;

            Title = " UrhoSharp + Xamarin.Forms";
            Content = new StackLayout
            {
                Padding = new Thickness(12, 12, 12, 40),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    urhoSurface,
                    restartBtn,
                    new Label { Text = "ROTATION::" },
                    rotationSlider,
                    new Label { Text = "SELECTED VALUE:" },
                    selectedBarSlider,
                }
            };
        }

        protected override void OnDisappearing()
        {
            UrhoSurface.OnDestroy();
            base.OnDisappearing();
        }

        void OnValuesSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (urhoApp?.SelectedBar != null)
                urhoApp.SelectedBar.Value = (float)e.NewValue;
        }

        private void OnBarSelection(Bar bar)
        {
            //reset value
            selectedBarSlider.ValueChanged -= OnValuesSliderValueChanged;
            selectedBarSlider.Value = bar.Value;
            selectedBarSlider.ValueChanged += OnValuesSliderValueChanged;
        }

        protected override async void OnAppearing()
        {
            StartUrhoApp();
        }

        async void StartUrhoApp()
        {
            urhoApp = await urhoSurface.Show<Charts>(new ApplicationOptions(assetsFolder: null) { Orientation = ApplicationOptions.OrientationType.LandscapeAndPortrait });
        }
    }
}
