using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Design_Style.Hellper
{
    public static class ToggleButtonHelper
    {
        public static ToggleButton GetButtonSource(UIElement uie) => (ToggleButton)uie.GetValue(ButtonSourceProperty);

        public static void SetButtonSource(UIElement obj, ToggleButton bttn) => obj.SetValue(ButtonSourceProperty, bttn);

        // Using a DependencyProperty as the backing store for IsRaiseEvent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonSourceProperty =
            DependencyProperty.RegisterAttached(
                nameof(GetButtonSource)[3..],
                typeof(ToggleButton),
                typeof(ToggleButtonHelper),
                new PropertyMetadata(null)
                {
                    PropertyChangedCallback = OnButtonSourceChanged
                });

        private static void OnButtonSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not UIElement uie)
            {
                throw new Exception("Только для UIElement.");
            }
            if (e.OldValue is ToggleButton old && TryGetHandler(uie, old, out RoutedEventHandler? handler))
            {
                old.Checked -= handler;
                old.Unchecked -= handler;
                old.Indeterminate -= handler;
            }
            if (e.NewValue is ToggleButton @new)
            {
                handler = GetHandler(uie, @new);

                @new.Checked += handler;
                @new.Unchecked += handler;
                @new.Indeterminate += handler;
            }
        }

        private static ConditionalWeakTable<UIElement, Dictionary<ToggleButton, RoutedEventHandler>> handlers = new();


        private static RoutedEventHandler GetHandler(UIElement uie, ToggleButton bttn)
        {
            if (!handlers.TryGetValue(uie, out var bttns))
            {
                bttns = new();
                handlers.Add(uie, bttns);
            }
            if (!bttns.TryGetValue(bttn, out var handler))
            {
                handler = (_, e) => OnChanged(uie, bttn, e);
                bttns.Add(bttn, handler);
            }
            return handler;
        }

        private static bool TryGetHandler(UIElement uie, ToggleButton bttn, out RoutedEventHandler? handler)
        {
            if (uie is not null && bttn is not null &&
                handlers.TryGetValue(uie, out var bttns)
                && bttns.TryGetValue(bttn, out handler))
            {
                return true;
            }
            handler = null;
            return false;
        }

        private static void OnChanged(UIElement uie, ToggleButton bttn, RoutedEventArgs e)
        {
            RoutedEvent routedEvent = null;
            if (e.RoutedEvent == ToggleButton.CheckedEvent)
            {
                routedEvent = ToggleButtonCheckedEvent;
            }
            else if (e.RoutedEvent == ToggleButton.UncheckedEvent)
            {
                routedEvent = ToggleButtonUncheckedEvent;
            }
            else if (e.RoutedEvent == ToggleButton.IndeterminateEvent)
            {
                routedEvent = ToggleButtonIndeterminateEvent;
            }
            else
            {
                throw new Exception("Только для Checked, Unchecked и Indeterminate");
            }

            uie.RaiseEvent(new RoutedEventArgs(routedEvent, uie));
        }

        #region Присоединённые события ToggleButton.
        //Извещает о изменении IsChecked ToggleButton. Нужны из-за того, чтобы избежать зацикливания событий.

        // Register a custom routed event using the bubble routing strategy.
        public static readonly RoutedEvent ToggleButtonCheckedEvent = EventManager.RegisterRoutedEvent(
            "ToggleButtonChecked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToggleButtonHelper));


        // Provide an add handler accessor method for the Clean event.
        public static void AddToggleButtonCheckedHandler(DependencyObject dependencyObject, PositionRoutedEventHandler handler)
        {
            if (dependencyObject is not UIElement uiElement)
                return;

            uiElement.AddHandler(ToggleButtonCheckedEvent, handler);
        }

        // Provide a remove handler accessor method for the Clean event.
        public static void RemoveToggleButtonCheckedHandler(DependencyObject dependencyObject, PositionRoutedEventHandler handler)
        {
            if (dependencyObject is not UIElement uiElement)
                return;

            uiElement.RemoveHandler(ToggleButtonCheckedEvent, handler);
        }

        // Register a custom routed event using the bubble routing strategy.
        public static readonly RoutedEvent ToggleButtonUncheckedEvent = EventManager.RegisterRoutedEvent(
            "ToggleButtonUnchecked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToggleButtonHelper));


        // Provide an add handler accessor method for the Clean event.
        public static void AddToggleButtonUncheckedHandler(DependencyObject dependencyObject, PositionRoutedEventHandler handler)
        {
            if (dependencyObject is not UIElement uiElement)
                return;

            uiElement.AddHandler(ToggleButtonUncheckedEvent, handler);
        }

        // Provide a remove handler accessor method for the Clean event.
        public static void RemoveToggleButtonUncheckedHandler(DependencyObject dependencyObject, PositionRoutedEventHandler handler)
        {
            if (dependencyObject is not UIElement uiElement)
                return;

            uiElement.RemoveHandler(ToggleButtonUncheckedEvent, handler);
        }

        // Register a custom routed event using the bubble routing strategy.
        public static readonly RoutedEvent ToggleButtonIndeterminateEvent = EventManager.RegisterRoutedEvent(
            "ToggleButtonIndeterminate", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToggleButtonHelper));


        // Provide an add handler accessor method for the Clean event.
        public static void AddToggleButtonIndeterminateHandler(DependencyObject dependencyObject, PositionRoutedEventHandler handler)
        {
            if (dependencyObject is not UIElement uiElement)
                return;

            uiElement.AddHandler(ToggleButtonIndeterminateEvent, handler);
        }

        // Provide a remove handler accessor method for the Clean event.
        public static void RemoveToggleButtonIndeterminateHandler(DependencyObject dependencyObject, PositionRoutedEventHandler handler)
        {
            if (dependencyObject is not UIElement uiElement)
                return;

            uiElement.RemoveHandler(ToggleButtonIndeterminateEvent, handler);
        }
        #endregion
    }
}
