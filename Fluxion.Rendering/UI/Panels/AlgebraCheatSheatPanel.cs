// Fluxion.Rendering/UI/Panels/AlgebraCheatSheetPanel.cs
using ImGuiNET;
using Fluxion.Math.Algebra.Concepts;

namespace Fluxion.Rendering.UI.Panels
{
    public sealed class AlgebraCheatSheetPanel
    {
        private readonly IReadOnlyList<AlgebraTopic> _topics;
        private bool _visible = true;

        public AlgebraCheatSheetPanel(IReadOnlyList<AlgebraTopic> topics)
        {
            _topics = topics;
        }

        public void Draw()
        {
            if (!_visible) return;

            ImGui.SetNextWindowSize(new System.Numerics.Vector2(360, 520), ImGuiCond.FirstUseEver);
            ImGui.Begin("Algebra Cheat Sheet", ref _visible, ImGuiWindowFlags.NoCollapse);

            foreach (var topic in _topics)
            {
                if (ImGui.CollapsingHeader(topic.Title, ImGuiTreeNodeFlags.DefaultOpen))
                {
                    foreach (var item in topic.Items)
                    {
                        ImGui.TextColored(new System.Numerics.Vector4(0.9f, 0.9f, 1f, 1f), item.Name);
                        ImGui.TextWrapped(item.Formula);
                        if (!string.IsNullOrWhiteSpace(item.Notes))
                        {
                            ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(0.8f, 0.8f, 0.8f, 1f));
                            ImGui.TextWrapped(item.Notes);
                            ImGui.PopStyleColor();
                        }
                        ImGui.Separator();
                    }
                }
            }

            ImGui.End();
        }
    }
}
