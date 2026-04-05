--SCRIPT INFO
local s_GUID = "RE2_DisableSharpeningAndTAA"
local s_version = "1.0.0"

local s_GUIDAndVVersion = s_GUID .. " v" .. s_version
local s_logPrefix = "[" .. s_GUID .. "] "
local s_configFileName = s_GUID .. ".lua.json"



--LOG
local function LogInfo(message)
	log.info(s_logPrefix .. message)
end

local function LogDebug(message)
	log.debug(s_logPrefix .. message)
end




--CONFIG
local tbl_config =
{
	b_enabled = true,
	b_fxaa = false
}

local function LoadFromJson()
	local tblLoadedConfig = json.load_file(s_configFileName)

	if tblLoadedConfig ~= nil then
        for key, val in pairs(tblLoadedConfig) do
            tbl_config[key] = val
        end
    end
end

local function SaveToJson()
	json.dump_file(s_configFileName, tbl_config)
end



--VARIABLES
local c_renderConfig = nil



--FUNCTIONS
local function ApplySettings()
	local tblConfig = tbl_config
	if tblConfig.b_enabled then
		local cRenderConfig = c_renderConfig
		if cRenderConfig ~= nil then
			local iAntiAliasing = 4
			if tblConfig.b_fxaa then iAntiAliasing = 0 end
			cRenderConfig:call("set_AnitiAliasingSetting",iAntiAliasing)
			cRenderConfig:call("set_AntiAliasing", iAntiAliasing)
			cRenderConfig:call("set_SharpnessSetting", 1)
		end
	end
end

local function EmptyPreHook(args) end

local function PostGetTemporalAA(retVal)
	if tbl_config.b_enabled then return sdk.to_ptr(5) end
end

local function PostGetSharpness(retVal)
	if tbl_config.b_enabled then return sdk.float_to_ptr(0.0) end
end


--CALLBACKS
local function PreBeginRendering()
	if c_renderConfig == nil then
		local noRenderer = sdk.get_native_singleton("via.render.Renderer")
		if noRenderer == nil then return end

		local tdRenderer = sdk.find_type_definition("via.render.Renderer")
		if tdRenderer == nil then return end

		c_renderConfig = sdk.call_native_func(noRenderer, tdRenderer, "get_RenderConfig")
		if c_renderConfig == nil then return end

		local tcToneMapping = sdk.find_type_definition("via.render.ToneMapping")
		if tcToneMapping == nil then return end

		local mGetTemporalAA = tcToneMapping:get_method("getTemporalAA")
		if mGetTemporalAA == nil then return end

		local mGetSharpness = tcToneMapping:get_method("get_Sharpness")
		if mGetSharpness == nil then return end

		sdk.hook(mGetTemporalAA, EmptyPreHook, PostGetTemporalAA)
		sdk.hook(mGetSharpness, EmptyPreHook, PostGetSharpness)
		ApplySettings()		
		LogInfo("Initialized")
	end
end



--MAIN
LoadFromJson()
re.on_config_save(SaveToJson)
re.on_pre_application_entry("BeginRendering", PreBeginRendering)
LogInfo("Loaded " .. s_version)



--SCRIPT GENERATED UI
local function combo(label, currentValue, names)
    local changed, newValue = imgui.combo(label, currentValue + 1, names)
    return changed, newValue - 1
end

re.on_draw_ui(function()
	if imgui.tree_node(s_GUIDAndVVersion) then
		local bChanged = false
		local bAnyChanged = false
		local tblConfig = tbl_config

		imgui.text("Note: Disabling the mod requires changing the in-game anti-aliasing option or game restart to revert the changes.")
		bChanged, tblConfig.b_enabled = imgui.checkbox("Enabled", tblConfig.b_enabled)
		bAnyChanged = bAnyChanged or bChanged
		bChanged, tblConfig.b_fxaa = imgui.checkbox("FXAA", tblConfig.b_fxaa)
		bAnyChanged = bAnyChanged or bChanged

		if bAnyChanged then ApplySettings() end

		imgui.tree_pop()
	end
end)