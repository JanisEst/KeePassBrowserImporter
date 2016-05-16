using KeePassLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;

namespace KeePassBrowserImporter
{
	public partial class ProviderForm : Form
	{
		public class GroupItem
		{
			public PwGroup Group { get; private set; }

			public int Level { get; private set; }

			public override string ToString()
			{
				var padding = new string(' ', Level * 2);
				return padding + Group.Name;
			}

			public GroupItem(PwGroup group, int level)
			{
				Group = group;
				Level = level;
			}
		}

		private List<RadioButton> browserRadioButtons;

		public IBrowserImporter SelectedProvider
		{
			get
			{
				//return browserRadioButtons.Where(r => r.Checked).FirstOrDefault()?.Tag as IBrowserImporter;
				var radio = browserRadioButtons.Where(r => r.Checked).FirstOrDefault();
				if (radio != null)
				{
					return radio.Tag as IBrowserImporter;
				}
				return null;
			}
		}

		public PwGroup SelectedGroup
		{
			get
			{
				//return (groupComboBox.SelectedItem as GroupItem)?.Group;
				var item = groupComboBox.SelectedItem as GroupItem;
				if (item != null)
				{
					return item.Group;
				}
				return null;
			}
		}

		public string SelectedProfile
		{
			get
			{
				return profileComboBox.SelectedItem as string;
			}
		}

		public string MasterPassword
		{
			get
			{
				return masterPasswordTextBox.Text;
			}
		}

		public bool ExtractTitle
		{
			get
			{
				return extractTitleCheckBox.Checked;
			}
		}

		public bool ExtractIcon
		{
			get
			{
				return extractIconCheckBox.Checked;
			}
		}

		public ProviderForm(PwDatabase database)
		{
			Contract.Requires(database != null);

			InitializeComponent();

			var providers = new List<IBrowserImporter>
			{
				new Firefox(),
				new InternetExplorer(),
				new Chrome(),
				new Opera(),
				new Chromium()
			};
			browserRadioButtons = new List<RadioButton>
			{
				firefoxRadioButton,
				internetExplorerRadioButton,
				chromeRadioButton,
				operaRadioButton,
				chromiumRadioButton
			};

			for (int i = 0; i < providers.Count; ++i)
			{
				if (providers[i].IsAvailable == false)
				{
					browserRadioButtons[i].Enabled = false;
				}

				browserRadioButtons[i].Tag = providers[i];
			}

			masterPasswordTextBox.UseSystemPasswordChar = true;

			FillGroupComboBox(database.RootGroup, 0);
			var preselectedGroup = database.LastSelectedGroup.Equals(PwUuid.Zero) ? database.RootGroup.Uuid : database.LastSelectedGroup;
			for (var i = 0; i < groupComboBox.Items.Count; ++i)
			{
				if ((groupComboBox.Items[i] as GroupItem).Group.Uuid.Equals(preselectedGroup))
				{
					groupComboBox.SelectedIndex = i;
					break;
				}
			}
		}

		private void FillGroupComboBox(PwGroup group, int level)
		{
			if (group == null)
			{
				return;
			}

			groupComboBox.Items.Add(new GroupItem(group, level));
			foreach (var sub in group.Groups)
			{
				FillGroupComboBox(sub, level + 1);
			}
		}

		private void SelectBrowser_CheckedChanged(object sender, EventArgs e)
		{
			var radio = sender as RadioButton;
			if (radio == null || radio.Tag as IBrowserImporter == null)
			{
				return;
			}

			if (radio.Checked == false)
			{
				return;
			}

			var provider = (IBrowserImporter)radio.Tag;

			profileGroupBox.Enabled = provider.SupportsMultipleProfiles;
			profileComboBox.Items.Clear();
			if (provider.SupportsMultipleProfiles)
			{
				var profiles = provider.GetProfiles().ToArray();
				profileComboBox.Items.AddRange(profiles);
				if (profiles.Length > 0)
				{
					profileComboBox.SelectedIndex = 0;
				}
			}

			masterPasswordGroupBox.Enabled = provider.UsesMasterPassword;
		}

		private void startButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
