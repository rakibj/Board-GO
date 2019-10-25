namespace _BoardGo.Scripts.Generic
{
    public interface IInput
    {
        float Horizontal { get; }
        float Vertical { get; }
        bool InputEnabled { get; set; }
        void GetInput();
    }
}
