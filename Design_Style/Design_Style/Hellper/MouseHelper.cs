using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Design_Style.Hellper
{
    public  class MouseHelper
    {
        private static object trueBox = true;
        private static object falseBox = false;
        public static bool GetIsDownPosition(UIElement uie)
        {
            return (bool)uie.GetValue(IsDownPositionProperty);
        }

        public static void SetIsDownPosition(UIElement uie, bool value)
        {
            uie.SetValue(IsDownPositionProperty, value ? trueBox : falseBox);
        }

        // Using a DependencyProperty as the backing store for IsDownPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDownPositionProperty =
            DependencyProperty.RegisterAttached(
                nameof(SetIsDownPosition)[3..],
                typeof(bool),
                typeof(MouseHelper),
                new PropertyMetadata(falseBox)
                {
                    PropertyChangedCallback = IsDownPositionChanged
                });

        private static void IsDownPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not UIElement uie)
            {
                throw new Exception("Только для UIElement.");
            }
            if (!Equals(e.OldValue, e.NewValue))
            {
                if (Equals(e.NewValue, trueBox))
                {
                    uie.AddHandler(UIElement.MouseDownEvent, (MouseButtonEventHandler)OnMouseDown, true);
                    //uie.MouseDown += OnMouseDown;
                }
                else
                {
                    uie.RemoveHandler(UIElement.MouseDownEvent, (MouseButtonEventHandler)OnMouseDown);
                    //uie.MouseDown -= OnMouseDown;
                    uie.ClearValue(LastDownPositionProperty);
                }
            }
        }

        private static void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            UIElement uie = (UIElement)sender;
            Point mousePos = e.GetPosition(uie);
            SetLastDownPosition(uie, mousePos);
            uie.RaiseEvent(new PositionRoutedEventArgs(LastMouseDownEvent, mousePos));
        }


        public static readonly DependencyProperty LastDownPositionProperty =
            DependencyProperty.RegisterAttached(
                nameof(GetLastDownPosition)[3..],
                typeof(Point),
                typeof(MouseHelper),
                new PropertyMetadata(new Point(0, 0)));

        public static Point GetLastDownPosition(UIElement element) => (Point)element.GetValue(LastDownPositionProperty);

        public static void SetLastDownPosition(UIElement element, Point value) => element.SetValue(LastDownPositionProperty, value);


        #region Присоединённое событие LastMouseDown.
        //Извещает о изменении LastDownPosition. Нужно из-за того,  что MouseDown может быть отменён дочерними элементами.

        // Register a custom routed event using the bubble routing strategy.
        public static readonly RoutedEvent LastMouseDownEvent = EventManager.RegisterRoutedEvent(
            "LastMouseDown", RoutingStrategy.Bubble, typeof(PositionRoutedEventHandler), typeof(MouseHelper));


        // Provide an add handler accessor method for the Clean event.
        public static void AddLastMouseDownHandler(DependencyObject dependencyObject, PositionRoutedEventHandler handler)
        {
            if (dependencyObject is not UIElement uiElement)
                return;

            uiElement.AddHandler(LastMouseDownEvent, handler);
        }

        // Provide a remove handler accessor method for the Clean event.
        public static void RemoveLastMouseDownHandler(DependencyObject dependencyObject, PositionRoutedEventHandler handler)
        {
            if (dependencyObject is not UIElement uiElement)
                return;

            uiElement.RemoveHandler(LastMouseDownEvent, handler);
        }
        #endregion
    }

    public class PositionRoutedEventArgs : RoutedEventArgs
    {
        public PositionRoutedEventArgs(Point? position)
            => Position = position;

        public PositionRoutedEventArgs(RoutedEvent routedEvent, Point? position)
            : base(routedEvent)
            => Position = position;

        public PositionRoutedEventArgs(RoutedEvent routedEvent, object source, Point? position)
            : base(routedEvent, source)
            => Position = position;
        public Point? Position { get; }
    }

    public delegate void PositionRoutedEventHandler (object sender, PositionRoutedEventArgs e);
}
