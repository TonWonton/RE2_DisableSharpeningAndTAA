#nullable enable
using System;
using Hexa.NET.ImGui;
using REFrameworkNET.Callbacks;
using REFrameworkNET.Attributes;
using REFrameworkNET;
using REFrameworkNETPluginConfig;
using REFrameworkNETPluginConfig.Utility;
using via.render;
using AntiAliasingType = via.render.RenderConfig.AntiAliasingType;


namespace RE2_DisableSharpeningAndTAA
{
	public class DisableSharpeningAndTAAPlugin
	{
		#region PLUGIN_INFO

		/*PLUGIN INFO*/
		public const string PLUGIN_NAME = "RE2_DisableSharpeningAndTAA";
		public const string COPYRIGHT = "";
		public const string COMPANY = "https://github.com/TonWonton/RE2_DisableSharpeningAndTAA";

		public const string GUID = "RE2_DisableSharpeningAndTAA";
		public const string VERSION = "1.0.0";

		public const string GUID_AND_V_VERSION = GUID + " v" + VERSION;

		#endregion



		/* VARIABLES */
		//Config
		private static Config _config = new Config(GUID);
		private static ConfigEntry<bool> _enabled = _config.Add("Enabled", true);
		private static ConfigEntry<bool> _fxaa = _config.Add("FXAA", false);

		//Variables
		private static RenderConfig? _renderConfig;
		private static object _resultObject = new object();



		/* METHODS */
		private static void ApplySettings()
		{
			if (_enabled.Value)
			{
				RenderConfig? renderConfig = _renderConfig;
				if (renderConfig != null)
				{
					bool isFXAA = _fxaa.Value;
					renderConfig.AnitiAliasingSetting = isFXAA ? AntiAliasingType.FXAA : AntiAliasingType.NONE;
					renderConfig.AntiAliasing = isFXAA ? AntiAliasingType.FXAA : AntiAliasingType.NONE;
					renderConfig.SharpnessSetting = RenderConfig.SharpnessType.Default;
				}
			}
		}

		[MethodHook(typeof(ToneMapping), "getTemporalAA", MethodHookType.Post)]
		public static void GetTemporalAA(ref ulong retVal)
		{
			if (_enabled.Value) retVal = (uint)ToneMapping.TemporalAA.Disable;
		}

		[MethodHook(typeof(ToneMapping), "get_Sharpness", MethodHookType.Post)]
		public static void GetSharpness(ref ulong retVal)
		{
			if (_enabled.Value) retVal = (retVal & 0xFFFFFFFF00000000) | (uint)BitConverter.SingleToInt32Bits(0f);
		}

		[Callback(typeof(BeginRendering), CallbackType.Pre)]
		public static void PreBeginRendering()
		{
			if (_renderConfig == null)
			{
				NativeObject? renderer = API.GetNativeSingleton("via.render.Renderer");
				if (renderer != null)
				{
					var rendererTypeDef = renderer.GetTypeDefinition();
					if (rendererTypeDef != null)
					{
						uint getRenderConfigIndex = rendererTypeDef.FindMethod("get_RenderConfig").GetIndex();
						object result = _resultObject;
						renderer.HandleInvokeMember_Internal(getRenderConfigIndex, null, ref result);

						if (result != null)
						{
							_renderConfig = ((ManagedObject)result).TryAs<RenderConfig>();
							if (_renderConfig != null)
							{
								//Apply once after initialization
								ApplySettings();
								Log.Info("Initialized");
							}
						}
					}
				}
			}
		}



		/* EVENT HANDLING */
		private static void OnSettingsChanged()
		{
			ApplySettings();
			_config.SaveToJson();
		}



		/* INITIALIZATION */
		[PluginEntryPoint]
		private static void Load()
		{
			foreach (ConfigEntryBase configEntry in _config.Values)
			{
				configEntry.ValueChanged += OnSettingsChanged;
			}

			_config.LoadFromJson();
			Log.Info("Loaded " + VERSION);
		}

		[PluginExitPoint]
		private static void Unload()
		{
			foreach (ConfigEntryBase configEntry in _config.Values)
			{
				configEntry.ValueChanged -= OnSettingsChanged;
			}

			Log.Info("Unloaded " + VERSION);
		}



		/* PLUGIN GENERATED UI */
		[Callback(typeof(ImGuiDrawUI), CallbackType.Pre)]
		public static void PreImGuiDrawUI()
		{
			if (API.IsDrawingUI() && ImGui.TreeNode(GUID_AND_V_VERSION))
			{
				int labelNr = 0;

				ImGui.Text("Note: Disabling the mod requires changing the in-game anti-aliasing option or game restart to revert the changes.");
				_enabled.Checkbox().ResetButton(ref labelNr);
				_fxaa.Checkbox().ResetButton(ref labelNr);
				ImGui.TreePop();
			}
		}
	}
}