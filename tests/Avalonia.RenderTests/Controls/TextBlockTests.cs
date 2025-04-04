using System.Net;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Layout;
using Avalonia.Media;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

#if AVALONIA_SKIA
namespace Avalonia.Skia.RenderTests
#else
namespace Avalonia.Direct2D1.RenderTests.Controls
#endif
{
    public class TextBlockTests : TestBase
    {
        public TextBlockTests()
            : base(@"Controls\TextBlock")
        {
        }

        [Win32Fact("Has text")]
        public async Task Should_Draw_TextDecorations()
        {
            Border target = new Border
            {
                Padding = new Thickness(8),
                Width = 200,
                Height = 30,
                Background = Brushes.White,
                Child = new TextBlock
                {
                    FontFamily = TestFontFamily,                  
                    FontSize = 12,
                    Foreground = Brushes.Black,
                    Text = "Neque porro quisquam est qui dolorem",
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.NoWrap,
                    TextDecorations = new TextDecorationCollection
                    {
                        new TextDecoration
                        {
                            Location = TextDecorationLocation.Overline,
                            StrokeThickness= 1.5,
                            StrokeThicknessUnit = TextDecorationUnit.Pixel,
                            Stroke = new SolidColorBrush(Colors.Red)
                        },
                        new TextDecoration
                        {
                            Location = TextDecorationLocation.Baseline,
                            StrokeThickness= 1.5,
                            StrokeThicknessUnit = TextDecorationUnit.Pixel,
                            Stroke = new SolidColorBrush(Colors.Green)
                        },
                        new TextDecoration
                        {
                            Location = TextDecorationLocation.Underline,
                            StrokeThickness= 1.5,
                            StrokeThicknessUnit = TextDecorationUnit.Pixel,
                            Stroke = new SolidColorBrush(Colors.Blue),
                            StrokeOffset = 2,
                            StrokeOffsetUnit = TextDecorationUnit.Pixel
                        }
                    }
                }
            };

            await RenderToFile(target);
            CompareImages();
        }

        [Win32Fact("Has text")]
        public async Task Wrapping_NoWrap()
        {
            Decorator target = new Decorator
            {
                Padding = new Thickness(8),
                Width = 200,
                Height = 200,
                Child = new TextBlock
                {
                    FontFamily = new FontFamily("Courier New"),
                    Background = Brushes.Red,
                    FontSize = 12,
                    Foreground = Brushes.Black,
                    Text = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit",
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.NoWrap,
                }
            };

            await RenderToFile(target);
            CompareImages();
        }


        [Win32Fact("Has text")]
        public async Task RestrictedHeight_VerticalAlign()
        {
            Control text(VerticalAlignment verticalAlignment, bool clip = true, bool restrictHeight = true)
            {
                return new Border()
                {
                    BorderBrush = Brushes.Blue,
                    BorderThickness = new Thickness(1),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Height = restrictHeight ? 20 : double.NaN,
                    Margin = new Thickness(1),
                    Child = new TextBlock
                    {
                        FontFamily = new FontFamily("Courier New"),
                        Background = Brushes.Red,
                        FontSize = 24,
                        Foreground = Brushes.Black,
                        Text = "L",
                        VerticalAlignment = verticalAlignment,
                        ClipToBounds = clip
                    }
                };
            }
            Decorator target = new Decorator
            {
                Padding = new Thickness(8),
                Width = 190,
                Height = 80,

                Child = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Children =
                    {
                        text(VerticalAlignment.Stretch, restrictHeight: false),
                        text(VerticalAlignment.Center),
                        text(VerticalAlignment.Stretch),
                        text(VerticalAlignment.Top),
                        text(VerticalAlignment.Bottom),
                        text(VerticalAlignment.Center, clip:false),
                        text(VerticalAlignment.Stretch, clip:false),
                        text(VerticalAlignment.Top, clip:false),
                        text(VerticalAlignment.Bottom, clip:false),
                    }
                }
            };

            await RenderToFile(target);
            CompareImages();
        }

        [Win32Fact("Has text")]
        public async Task Should_Draw_Run_With_Background()
        {
            Decorator target = new Decorator
            {
                Padding = new Thickness(8),
                Width = 200,
                Height = 50,
                Child = new TextBlock
                {
                    FontFamily = new FontFamily("Courier New"),
                    FontSize = 12,
                    Foreground = Brushes.Black,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.NoWrap,
                    Inlines = new InlineCollection
                    {
                        new Run
                        {
                            Text = "Neque porro quisquam",
                            Background = Brushes.Red
                        }
                    }
                }
            };

            await RenderToFile(target);
            CompareImages();
        }


        [InlineData(150, 200, TextWrapping.NoWrap)]
        [InlineData(44, 200, TextWrapping.NoWrap)]
        [InlineData(44, 400, TextWrapping.Wrap)]
        [Win32Theory("Has text")]
        public async Task Should_Measure_Arrange_TextBlock(double width, double height, TextWrapping textWrapping)
        {
            var text = "Hello World";

            var target = new StackPanel { Width = 200, Height = height };

            target.Children.Add(new TextBlock 
            { 
                Text = text, 
                Background = Brushes.Red, 
                HorizontalAlignment = HorizontalAlignment.Left, 
                TextAlignment = TextAlignment.Left,
                Width = width, 
                TextWrapping = textWrapping 
            });
            target.Children.Add(new TextBlock 
            { 
                Text = text,
                Background = Brushes.Red, 
                HorizontalAlignment = HorizontalAlignment.Left, 
                TextAlignment = TextAlignment.Center, 
                Width = width, 
                TextWrapping = textWrapping 
            });
            target.Children.Add(new TextBlock 
            { 
                Text = text, 
                Background = Brushes.Red, 
                HorizontalAlignment = HorizontalAlignment.Left, 
                TextAlignment = TextAlignment.Right, 
                Width = width, 
                TextWrapping = textWrapping 
            });

            target.Children.Add(new TextBlock 
            {
                Text = text,
                Background = Brushes.Red,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Left, 
                Width = width, 
                TextWrapping = textWrapping
            });
            target.Children.Add(new TextBlock 
            { 
                Text = text, 
                Background = Brushes.Red, 
                HorizontalAlignment = HorizontalAlignment.Center, 
                TextAlignment = TextAlignment.Center, 
                Width = width,
                TextWrapping = textWrapping
            });
            target.Children.Add(new TextBlock 
            { 
                Text = text,
                Background = Brushes.Red,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Right, 
                Width = width,
                TextWrapping = textWrapping 
            });

            target.Children.Add(new TextBlock 
            { 
                Text = text,
                Background = Brushes.Red, 
                HorizontalAlignment = HorizontalAlignment.Right,
                TextAlignment = TextAlignment.Left, 
                Width = width, 
                TextWrapping = textWrapping
            });
            target.Children.Add(new TextBlock 
            { 
                Text = text, 
                Background = Brushes.Red, 
                HorizontalAlignment = HorizontalAlignment.Right, 
                TextAlignment = TextAlignment.Center, 
                Width = width, 
                TextWrapping = textWrapping 
            });
            target.Children.Add(new TextBlock 
            { 
                Text = text, 
                Background = Brushes.Red, 
                HorizontalAlignment = HorizontalAlignment.Right, 
                TextAlignment = TextAlignment.Right, 
                Width = width, 
                TextWrapping = textWrapping 
            });

            var testName = $"Should_Measure_Arrange_TextBlock_{width}_{textWrapping}";

            await RenderToFile(target, testName);
            CompareImages(testName);
        }
    }
}
