
namespace Winger.Input.Event
{
    public interface InputEventHandler
    {
        void HandleEvent(UserInput input, InputEvent e, InputEventType type);
    }
}
