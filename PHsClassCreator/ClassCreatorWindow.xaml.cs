using System;
using System.CodeDom.Compiler;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace PHsClassCreator
{
	/// <summary>
	/// MainWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ClassCreatorWindow : Window, IWin32Window
	{
		private static char[] vSeparator = new char[] { '.' };

		public IntPtr Handle { get { return this.pHandle; } }

		private IntPtr pHandle;
		private CodeDomProvider sCompiler;
		private FolderBrowserDialog sFolderFinder;
		private System.Windows.Media.Effects.Effect sBlurEffect;

		public ClassCreatorWindow()
		{
			this.InitializeComponent();

			this.sCompiler = CodeDomProvider.CreateProvider("C++");

			//Initialize folder finder.
			this.sFolderFinder = new FolderBrowserDialog();
			this.sFolderFinder.ShowNewFolderButton = true;
			this.sFolderFinder.Description = "Select your project's source folder.";

			this.sBlurEffect = new System.Windows.Media.Effects.BlurEffect
			{
				KernelType = System.Windows.Media.Effects.KernelType.Gaussian,
				Radius = 5f,
				RenderingBias = System.Windows.Media.Effects.RenderingBias.Performance
			};
		}

		private void OnWindowLoaded(object sender, RoutedEventArgs e)
		{
			this.MinHeight = this.MaxHeight = this.ActualHeight;
			this.pHandle = new System.Windows.Interop.WindowInteropHelper(this).Handle;

			//Init component state.
			this.updateLocation();
			this.updateFileName();
			this.updateIncludeGuardName();

			e.Handled = true;
		}

		private void OnFindButtonClick(object sender, RoutedEventArgs e)
		{
			this.updateLocation();

			e.Handled = true;
		}

		private void OnNameChanged(object sender, TextChangedEventArgs e)
		{
			if (this.IsLoaded)
			{
				this.updateCreationButton();
				this.updateFileName();
				this.updateIncludeGuardName();
			}

			e.Handled = true;
		}

		private void OnTypeChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.IsLoaded)
				this.updateIncludeGuardName();

			e.Handled = true;
		}

		private void OnCreateButtonClick(object sender, RoutedEventArgs e)
		{
			//Create a class or an interface according to the selected type.
			bool bResult;

			switch (this.creationTypeCombo.SelectedIndex)
			{
				default:
				case 0:
				{
					bResult = Factory.createClass(
						this.sFolderFinder.SelectedPath,
						this.creationFileNameText.Text,
						this.creationNameText.Text,
						this.creationIncludeGuardCheck.IsChecked.Value ? this.creationIncludeGuardText.Text : null);
				}
				break;
				case 1:
				{
					bResult = Factory.createTemplateClass(
						this.sFolderFinder.SelectedPath,
						this.creationFileNameText.Text,
						this.creationNameText.Text,
						this.creationIncludeGuardCheck.IsChecked.Value ? this.creationIncludeGuardText.Text : null);
				}
				break;
				case 2:
				{
					bResult = Factory.createInterface(
						this.sFolderFinder.SelectedPath,
						this.creationFileNameText.Text,
						this.creationNameText.Text,
						this.creationIncludeGuardCheck.IsChecked.Value ? this.creationIncludeGuardText.Text : null);
				}
				break;
			}

			if (bResult)
				System.Windows.Forms.MessageBox.Show(this, "The files are successfully created.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
			else
				System.Windows.Forms.MessageBox.Show(this, "An error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

			e.Handled = true;
		}

		private void OnGuardCheckedChanged(object sender, RoutedEventArgs e)
		{
			if (this.IsLoaded)
				this.updateIncludeGuard();

			e.Handled = true;
		}

		private void updateLocation()
		{
			this.sFolderFinder.ShowDialog(this);
			this.projectLocation.ToolTip = this.projectLocationBlock.Text = this.sFolderFinder.SelectedPath.Length == 0 ? "N/A" : this.sFolderFinder.SelectedPath;

			this.updateCreationButton();
		}

		private bool updateCreationButton()
		{
			if (this.sFolderFinder.SelectedPath.Length == 0)
			{
				this.creationError.Foreground = System.Windows.Media.Brushes.Red;
				this.creationError.Content = "The project path must be set.";
				this.creationButton.Effect = this.sBlurEffect;
				this.creationButton.IsEnabled = false;
				return false;
			}

			if (this.creationNameText.Text.Length == 0)
			{
				this.creationError.Foreground = System.Windows.Media.Brushes.Red;
				this.creationError.Content = "The name of target cannot be empty.";
				this.creationButton.Effect = this.sBlurEffect;
				this.creationButton.IsEnabled = false;
				return false;
			}

			if (!this.sCompiler.IsValidIdentifier(this.creationNameText.Text))
			{
				this.creationError.Foreground = System.Windows.Media.Brushes.Red;
				this.creationError.Content = "Invalid target name.";
				this.creationButton.Effect = this.sBlurEffect;
				this.creationButton.IsEnabled = false;
				return false;
			}

			//Check the namespace.
			if (this.creationNamespaceText.Text.Length > 0)
			{
				string[] vList = this.creationNamespaceText.Text.Split(ClassCreatorWindow.vSeparator, StringSplitOptions.None);

				foreach (string sIdentifier in vList)
				{
					if (sIdentifier.Length == 0 || !this.sCompiler.IsValidIdentifier(sIdentifier))
					{
						this.creationError.Foreground = System.Windows.Media.Brushes.Red;
						this.creationError.Content = "Invalid target namespace.";
						this.creationButton.Effect = this.sBlurEffect;
						this.creationButton.IsEnabled = false;
						return false;
					}
				}
			}

			this.creationError.Foreground = System.Windows.Media.Brushes.Black;
			this.creationError.Content = "Ready.";
			this.creationButton.Effect = null;
			this.creationButton.IsEnabled = true;

			return true;
		}

		private void updateIncludeGuard()
		{
			this.creationIncludeGuard.Effect = this.creationIncludeGuardText.Effect =
				(this.creationIncludeGuard.IsEnabled = this.creationIncludeGuardText.IsEnabled = this.creationIncludeGuardCheck.IsChecked.Value) ?
				null :
				this.sBlurEffect;
		}

		private void updateFileName()
		{
			this.creationFileNameText.Text = this.creationNameText.Text;
		}

		private void updateIncludeGuardName()
		{
			this.creationIncludeGuardText.Text = "_";

			if(this.creationTypeCombo.SelectedIndex == 0 || this.creationTypeCombo.SelectedIndex == 1)
				this.creationIncludeGuardText.Text += "CLASS_";
			else if (this.creationTypeCombo.SelectedIndex == 2)
				this.creationIncludeGuardText.Text += "INTERFACE_";

			string[] vList = this.creationNamespaceText.Text.Split(ClassCreatorWindow.vSeparator, StringSplitOptions.RemoveEmptyEntries);

			foreach (string sIdentifier in vList)
			{
				this.creationIncludeGuardText.Text += sIdentifier.ToUpper();
				this.creationIncludeGuardText.Text += "_";
			}

			if (this.creationNameText.Text.Length == 0)
				return;

			this.creationIncludeGuardText.Text += char.ToUpper(this.creationNameText.Text[0]);

			for (int nIndex = 1, nLength = this.creationNameText.Text.Length; nIndex < nLength; ++nIndex)
			{
				if (char.IsUpper(this.creationNameText.Text[nIndex]))
					this.creationIncludeGuardText.Text += "_";

				this.creationIncludeGuardText.Text += char.ToUpper(this.creationNameText.Text[nIndex]);
			}
		}
	}
}