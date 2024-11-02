namespace CorpusExplorer.v3.Sdk.Interaction
{
  public class InteractionMessage
  {
    public InteractionMessage(string message, string title, InteractionMessageType type = InteractionMessageType.Information, InteractionMessageAction action = InteractionMessageAction.Ok)
    {
      Message = message;
      Title = title;
      Type = type;
      Action = action;
    }

    public string Message { get; }
    public string Title { get; }
    public InteractionMessageType Type { get; }
    public InteractionMessageAction Action { get; }
  }
}
