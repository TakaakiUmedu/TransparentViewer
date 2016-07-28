using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;


namespace TransparentViewer {
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();

//			this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
			this.image.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
			this.dialog = new OpenFileDialog();
		}

		OpenFileDialog dialog;
		FileInfo current_file;
		private void Load(FileInfo file) {
			var src = new BitmapImage();
			src.BeginInit();
			src.UriSource = new Uri(file.FullName);
			src.EndInit();
			this.image.Source = src;
			current_file = file;
		}
		private void Button_Click(object sender, RoutedEventArgs e) {
			if (this.dialog.ShowDialog() == true) {
				Load(new FileInfo(this.dialog.FileName));
				image.Focus();
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e) {
			this.Close();
		}

		private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			this.image.Opacity = this.slider.Value / 100.0;
		}

		private void Window_KeyDown(object sender, KeyEventArgs e) {
			Boolean handled = true;
			switch (e.Key) {
				case Key.Space:
					if (current_file != null) {
						DirectoryInfo dir = current_file.Directory;
						var files = new List<FileInfo>();
						foreach (var file in dir.EnumerateFiles()) {
							var ext = file.Extension.ToLower();
							if (ext == ".png" || ext == ".jpg" || ext == ".bmp") {
								files.Add(file);
							}
						}
						if (files.Count > 1) {
							files.Sort((a, b) => a.Name.CompareTo(b.Name));
							var current_index = files.FindIndex((file) => current_file.FullName == file.FullName);
							var next_index = current_index;
							if (next_index < 0) {
								next_index = 0;
							} else {
								if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) {
									next_index--;
								} else {
									next_index++;
								}
								if (next_index >= files.Count) {
									next_index = 0;
								} else if (next_index < 0) {
									next_index = files.Count - 1;
								}
							}
							Load(files[next_index]);
						}
					}
					break;
				case Key.Left:
					this.Left--;
					break;
				case Key.Right:
					this.Left++;
					break;
				case Key.Up:
					this.Top--;
					break;
				case Key.Down:
					this.Top++;
					break;
				case Key.OemPlus:
				case Key.Add:
					this.Width++;
					break;
				case Key.OemMinus:
				case Key.Subtract:
					this.Width--;
					break;
				default:
					handled = false;
					break;
			}
			if (handled) {
				e.Handled = true;
			}
		}

		private void image_MouseUp(object sender, MouseButtonEventArgs e) {
			this.image.Focus();
		}
		

	}
}
