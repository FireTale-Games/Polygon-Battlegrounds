namespace FTS.UI
{
    public interface IButtonHandler<in T>
    {
        public void OnEnter(T t);
        public void OnExit(T t);
        public void OnClick(T t);
    }
}