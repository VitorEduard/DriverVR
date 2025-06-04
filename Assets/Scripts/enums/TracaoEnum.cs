
using System.Diagnostics;

public class TracaoEnum
{
    public static readonly TracaoEnum QUADRO_RODAS = new TracaoEnum(4, 0, 4);
    public static readonly TracaoEnum DIANTEIRA = new TracaoEnum(2, 0, 4);
    public static readonly TracaoEnum TRASEIRA = new TracaoEnum(2, 2, 4);

    public int qtdRodas { get; }
    public int inicioRodas { get; }
    public int fimRodas { get; }

    private TracaoEnum(int qtd, int inicio, int fim)
    {
        qtdRodas = qtd;
        inicioRodas = inicio;
        fimRodas = fim;
    }
}