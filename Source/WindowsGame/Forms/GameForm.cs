﻿using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

using Junior.Common;

using TextAdventure.Engine.Objects;
using TextAdventure.WindowsGame.Configuration;
using TextAdventure.WindowsGame.Helpers;

namespace TextAdventure.WindowsGame.Forms
{
	public partial class GameForm : Form
	{
		private static readonly FpsConfigurationSection _fpsConfigurationSection = (FpsConfigurationSection)ConfigurationManager.GetSection("fps");
		private static readonly LogConfigurationSection _logConfigurationSection = (LogConfigurationSection)ConfigurationManager.GetSection("log");
		private static readonly WorldTimeConfigurationSection _worldTimeConfigurationSection = (WorldTimeConfigurationSection)ConfigurationManager.GetSection("worldTime");
		private TextAdventureGame _game;
		private Player _startingPlayer;
		private Engine.Objects.World _world;

		public GameForm(Engine.Objects.World world, Player startingPlayer)
		{
			_world = world;
			_startingPlayer = startingPlayer;
			world.ThrowIfNull("world");
			startingPlayer.ThrowIfNull("startingPlayer");

			InitializeComponent();
			SetNormalViewSize();

			fpsToolStripMenuItem.Checked = _fpsConfigurationSection.Visible;
			logToolStripMenuItem.Checked = _logConfigurationSection.Visible;
			worldTimeToolStripMenuItem.Checked = _worldTimeConfigurationSection.Visible;
		}

		public void Render()
		{
			if (_game != null)
			{
				_game.Tick();
			}
		}

		private void SetNormalViewSize()
		{
			Size = new Size(
				Width + (Constants.GameWindow.PreferredBackBufferWidth - xnaControl.Size.Width),
				Height + (Constants.GameWindow.PreferredBackBufferHeight - xnaControl.Size.Height));
		}

		private void OpenGame(string path)
		{
			try
			{
				Engine.Objects.World world = WorldLoader.Instance.FromAssembly(path);

				LoadGame(new TextAdventureGame(xnaControl.GraphicsDevice, world, world.StartingPlayer, _fpsConfigurationSection, _logConfigurationSection, _worldTimeConfigurationSection), world);
			}
			catch (Exception exception)
			{
				MessageBoxHelper.Instance.ShowError(this, String.Format("Error opening game:{0}{0}{1}", Environment.NewLine, exception.Message));
			}
		}

		private void CloseGame()
		{
			if (_game == null)
			{
				return;
			}

			_game.Dispose();
			_game = null;
		}

		private void LoadGame(TextAdventureGame game, Engine.Objects.World world)
		{
			CloseGame();

			Text = world.Title + " - Text Adventure";
			_game = game;
			_game.Run();
		}

		protected override void OnShown(EventArgs e)
		{
			SetNormalViewSize();
			xnaControl.CreateGraphicsDevice();
			LoadGame(new TextAdventureGame(xnaControl.GraphicsDevice, _world, _startingPlayer, _fpsConfigurationSection, _logConfigurationSection, _worldTimeConfigurationSection), _world);
			_world = null;
			_startingPlayer = null;

			base.OnShown(e);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			CloseGame();

			base.OnFormClosing(e);
		}

		protected override void OnResizeBegin(EventArgs e)
		{
			_game.Pause();

			base.OnResizeBegin(e);
		}

		protected override void OnResizeEnd(EventArgs e)
		{
			_game.Unpause();

			base.OnResizeEnd(e);
		}

		private void ExitToolStripMenuItemOnClick(object sender, EventArgs e)
		{
			Close();
		}

		private void NormalSizeToolStripMenuItemOnClick(object sender, EventArgs e)
		{
			SetNormalViewSize();
		}

		private void FpsToolStripMenuItemOnCheckedChanged(object sender, EventArgs e)
		{
			_fpsConfigurationSection.Visible = fpsToolStripMenuItem.Checked;
		}

		private void LogToolStripMenuItemOnClick(object sender, EventArgs e)
		{
			_logConfigurationSection.Visible = logToolStripMenuItem.Checked;
		}

		private void WorldTimeToolStripMenuItemOnClick(object sender, EventArgs e)
		{
			_worldTimeConfigurationSection.Visible = worldTimeToolStripMenuItem.Checked;
		}

		private void OpenToolStripMenuItemOnClick(object sender, EventArgs e)
		{
			_game.Pause();

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				OpenGame(openFileDialog.FileName);
			}
			else
			{
				_game.Unpause();
			}
		}

		private void MenuStripOnMenuActivate(object sender, EventArgs e)
		{
			if (_game != null)
			{
				_game.Pause();
			}

			fpsToolStripMenuItem.Enabled = _game != null;
			logToolStripMenuItem.Enabled = _game != null;
			worldTimeToolStripMenuItem.Enabled = _game != null;
		}

		private void MenuStripOnMenuDeactivate(object sender, EventArgs e)
		{
			if (_game != null)
			{
				_game.Unpause();
			}
		}
	}
}