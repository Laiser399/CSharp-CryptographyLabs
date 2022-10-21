namespace Module.DES.Services.Abstract;

public interface IFeistelFunctionService
{
    uint Calculate(uint value, ulong roundKey48);
}