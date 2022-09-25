namespace Module.RSA.Services.Abstract;

public interface IRoundCountCalculator
{
    /// <summary>
    /// Вычисляет кол-во раундов проверки числа на простоту, необходимое, чтобы достичь нужной вероятности
    /// </summary>
    /// <param name="probability">Желаемая вероятность того, что при результате "число - простое" число действительно является простым</param>
    /// <param name="singleRoundWrongResultProbability">Вероятность при одной проверке того, что в случае результата "число - простое" число в действительности является составным</param>
    int GetRoundCount(double probability, double singleRoundWrongResultProbability);
}