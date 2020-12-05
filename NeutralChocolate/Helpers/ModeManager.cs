using System.Collections.Generic;

namespace NeutralChocolate
{
    public enum Mode
    {
        Normal,
        Debug,
        Editor
    }

    public enum DebugOptions
    {
        None,
        ShowLocators,
        FrameAdvance,
    }

    public class ModeManager
    {
        public Mode currentMode = Mode.Normal;
        public HashSet<DebugOptions> enabledOptions = new HashSet<DebugOptions>();

        public void ToggleDebugOption(DebugOptions option)
        {
            if (enabledOptions.Contains(option))
            {
                enabledOptions.Remove(option);
            }
            else
            {
                enabledOptions.Add(option);
                currentMode = Mode.Debug;
            }
        }

        public bool Active(DebugOptions option) {
            return currentMode == Mode.Debug && enabledOptions.Contains(option);
        }

        public void Toggle(Mode mode)
        {
            if (mode == Mode.Normal || currentMode == mode)
                currentMode = Mode.Normal;
            else
                currentMode = mode;
        }
    }
}
