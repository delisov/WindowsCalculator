using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace CalcDmitriyElisov
{
    public class MyRevealBorderBrushExtension : MarkupExtension
    {
        private Color fallbackColor = Colors.White;

        /// <summary>
        /// The color to use for rendering in case the <see cref="MarkupExtension"/> can't work correctly.
        /// </summary>
        public Color FallbackColor
        {
            get
            {
                return fallbackColor;
            }
            set
            {
                fallbackColor = value;
            }
        }

        private Color color = Colors.White;
        /// <summary>
        /// Gets or sets a value that specifies the base background color for the brush.
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
        private Transform transform = Transform.Identity;

        public Transform Transform
        {
            get
            {
                return transform;
            }
            set
            {
                transform = value;
            }
        }
        private Transform relativeTransform = Transform.Identity;

        public Transform RelativeTransform
        {
            get
            {
                return relativeTransform;
            }
            set
            {
                relativeTransform = value;
            }
        }
        private double opacity = 1.0;
        public double Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                opacity = value;
            }
        }
        private double radius = 100.0;
        public double Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // 如果没有服务，则直接返回。
            if (!(serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget)) 
                return null;
            var service = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            // MarkupExtension 在样式模板中，返回 this 以延迟提供值。
            if (service.TargetObject.GetType().Name.EndsWith("SharedDp")) return this;
            if (!(service.TargetObject is FrameworkElement)) return this;
            var element = service.TargetObject as FrameworkElement;
            if (DesignerProperties.GetIsInDesignMode(element)) return new SolidColorBrush(FallbackColor);

            var window = Application.Current.MainWindow; 
            if (window == null) return this;
            var brush = CreateBrush(window, element);
            return brush;
        }

        private class MouseMovementHandler{
            private FrameworkElement element;
            private RadialGradientBrush brush;
            private Window window;
            public MouseMovementHandler(FrameworkElement element, RadialGradientBrush brush, Window window)
            {
                this.element = element;
                this.brush = brush;
                this.window = window;
            }
            public void OnMouseMove(object sender, MouseEventArgs e)
            {
                var position = e.GetPosition(element);
                brush.GradientOrigin = position;
                brush.Center = position;
            }

            public void OnClosed(object o, EventArgs eventArgs)
            {
                window.MouseMove -= OnMouseMove;
                window.Closed -= OnClosed;
            }
        }
        private Brush CreateBrush(Window window, FrameworkElement element)
        {
            var brush = new RadialGradientBrush(Color, Colors.Transparent)
            {
                MappingMode = BrushMappingMode.Absolute,
                RadiusX = Radius,
                RadiusY = Radius,
                Opacity = Opacity,
                Transform = Transform,
                RelativeTransform = RelativeTransform,
                Center = new Point(double.NegativeInfinity, double.NegativeInfinity),
            };
            var handler = new MouseMovementHandler(element, brush, window);
            window.MouseMove += handler.OnMouseMove;
            window.Closed += handler.OnClosed;
            return brush;
        }
    }
}
