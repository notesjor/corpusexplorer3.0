namespace CorpusExplorer.v3.Sdk.Interaction
{
  public static class UniversalInteractionController
  {
    public delegate void InteractionHandler(InteractionMessage message);

    private static List<InteractionHandler> _handlers = [];

    public static void RegisterHandler(InteractionHandler handler) => _handlers.Add(handler);

    public static void ClearAllHandler() => _handlers.Clear();

    public static void Send(InteractionMessage message) => _handlers.ForEach(h => h(message));

    public static void Send(string message, string title, InteractionMessageType type = InteractionMessageType.Information, InteractionMessageAction action = InteractionMessageAction.Ok) 
      => Send(new InteractionMessage(message, title, type, action));
  }
}
