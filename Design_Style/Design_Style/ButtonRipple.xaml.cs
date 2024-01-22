using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Design_Style
{
    public class ButtonRipple
    {
        #region RippleColor цвет анимации Ripple effect
        public static readonly DependencyProperty RippleColorProperty =
            DependencyProperty.RegisterAttached(
                nameof(GetRippleColor)[3..],
                typeof(Brush),
                typeof(ButtonRipple),
                new PropertyMetadata(Brushes.White));

        public static Brush GetRippleColor(UIElement element) => (Brush)element.GetValue(RippleColorProperty);

        public static void SetRippleColor(UIElement element, Brush value) => element.SetValue(RippleColorProperty, value);
        #endregion

        #region BackgroundMouseOver цвет фона при наведении мыши
        public static readonly DependencyProperty BackgroundMouseOverProperty =
            DependencyProperty.RegisterAttached(
                nameof(GetBackgroundMouseOver)[3..],
                typeof(SolidColorBrush),
                typeof(ButtonRipple),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BBBBBB"))));

        public static SolidColorBrush GetBackgroundMouseOver(UIElement element) => (SolidColorBrush)element.GetValue(BackgroundMouseOverProperty);

        public static void SetBackgroundMouseOver(UIElement element, SolidColorBrush value) => element.SetValue(BackgroundMouseOverProperty, value);
        #endregion

        #region CornerRadius
        public static readonly DependencyProperty CornerRadiusButtonProperty =
            DependencyProperty.RegisterAttached(
                nameof(GetCornerRadiusButton)[3..],
                typeof(double),
                typeof(ButtonRipple),
                new PropertyMetadata((double)6));

        public static double GetCornerRadiusButton(UIElement element) => (double)element.GetValue(CornerRadiusButtonProperty);

        public static void SetCornerRadiusButton(UIElement element, double value) => element.SetValue(CornerRadiusButtonProperty, value);
        #endregion
    }
}
