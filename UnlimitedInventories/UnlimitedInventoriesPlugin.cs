﻿using System;
using System.IO;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Newtonsoft.Json;

namespace UnlimitedInventories
{
	/// <summary>
	/// Represents the UnlimitedInventories plugin.
	/// </summary>
	[ApiVersion(2, 1)]
	public sealed class UnlimitedInventoriesPlugin : TerrariaPlugin
	{
		private static readonly string ConfigPath = Path.Combine(TShock.SavePath, "unlimitedinventoriesconfig.json");

		private readonly UnlimitedInventoriesCommands _commands;
		private readonly Database _database;

		/// <summary>
		/// Gets the plugin's author.
		/// </summary>
		public override string Author => "Professor X";

		/// <summary>
		/// Gets the plugin's description.
		/// </summary>
		public override string Description => "Allows live inventory switching.";

		/// <summary>
		/// Gets the plugin's name.
		/// </summary>
		public override string Name => "UnlimitedInventories";

		/// <summary>
		/// Gets the plugin's version.
		/// </summary>
		public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

		/// <summary>
		/// Initializes a new instance of the <see cref="UnlimitedInventoriesPlugin"/> class with the specified Main instance.
		/// </summary>
		/// <param name="game">The Main instance.</param>
		public UnlimitedInventoriesPlugin(Main game) : base(game)
		{
			_database = new Database();
			_commands = new UnlimitedInventoriesCommands(_database);
		}

		/// <summary>
		/// Disposes the plugin.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release unmanaged resources; otherwise, <c>false</c>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_commands.Deregister();
				File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Config.Instance, Formatting.Indented));
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Initializes the plugin.
		/// </summary>
		public override void Initialize()
		{
			if (File.Exists(ConfigPath))
			{
				Config.Instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigPath));
			}

			_database.ConnectDatabase();
			_commands.Register();
		}
	}
}
