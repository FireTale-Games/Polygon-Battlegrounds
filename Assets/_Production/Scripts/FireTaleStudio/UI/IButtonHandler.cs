namespace FTS.UI
{
    public interface IButtonHandler<in T>
    {
        public void Enter(T t);
        public void Exit(T t);
        public void Press(T t);
    }
}