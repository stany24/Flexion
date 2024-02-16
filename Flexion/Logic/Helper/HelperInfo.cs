using System.Collections.Generic;

namespace Flexion.Logic.Helper;

public static class HelperInfo
{
    public static List<IHelperModule> MainWindowModules { get; } = new()
    {
        new HelperText("MainWindowHelper1Binding"),
        new HelperButton("MainWindowHelper2Binding","https://docs.avaloniaui.net/docs/guides/data-binding/binding-from-code"),
        new HelperImage("MainWindow/test.png")
    };
    
    public static List<IHelperModule> ForceWindowModules { get; } = new()
    {
        new HelperText("MainWindowHelper1Binding"),
        new HelperButton("MainWindowHelper2Binding","https://docs.avaloniaui.net/docs/guides/data-binding/binding-from-code"),
        new HelperImage("MainWindow/test.png")
    };
    
    public static List<IHelperModule> MaterialWindowModules { get; } = new()
    {
        new HelperText("MaterialEditorHelper1Binding"),
        new HelperText("MaterialEditorHelper2Binding"),
        new HelperButton("MaterialEditorHelper3Binding","https://docs.avaloniaui.net/docs/guides/data-binding/binding-from-code")
    };
    
    public static List<IHelperModule> LayerWindowModules { get; } = new()
    {
        new HelperText("LayerEditorHelper1Binding"),
        new HelperText("LayerEditorHelper2Binding"),
        new HelperText("LayerEditorHelper3Binding"),
        new HelperText("LayerEditorHelper4Binding"),
        new HelperText("LayerEditorHelper5Binding"),
        new HelperText("LayerEditorHelper6Binding"),
        new HelperText("LayerEditorHelper7Binding"),
        new HelperButton("LayerEditorHelper8Binding","https://docs.avaloniaui.net/docs/guides/data-binding/binding-from-code")
    };
    
    public static List<IHelperModule> PieceWindowModules { get; } = new()
    {
        new HelperText("MainWindowHelper1Binding"),
        new HelperButton("MainWindowHelper2Binding","https://docs.avaloniaui.net/docs/guides/data-binding/binding-from-code"),
        new HelperImage("MainWindow/test.png")
    };
    public static List<IHelperModule> PieceLayerWindowModules { get; } = new()
    {
        new HelperText("MainWindowHelper1Binding"),
        new HelperButton("MainWindowHelper2Binding","https://docs.avaloniaui.net/docs/guides/data-binding/binding-from-code"),
        new HelperImage("MainWindow/test.png")
    };
}