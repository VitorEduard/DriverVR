
public class MarchaEnum {
    public static readonly MarchaEnum RE = new MarchaEnum("Ré", -380, 3.2f);
    public static readonly MarchaEnum NEUTRO = new MarchaEnum("Neutro", 0, 0.0f);
    public static readonly MarchaEnum PRIMEIRA = new MarchaEnum("1ª", 280, 4f);
    public static readonly MarchaEnum SEGUNDA = new MarchaEnum("2ª", 300, 2.2f);
    public static readonly MarchaEnum TERCEIRA = new MarchaEnum("3ª", 330, 1.4f);
    public static readonly MarchaEnum QUARTA = new MarchaEnum("4ª", 360, 1.0f);
    public static readonly MarchaEnum QUINTA = new MarchaEnum("5ª", 380, 0.8f);
    public static readonly MarchaEnum SEXTA = new MarchaEnum("6ª", 400, 0.7f);

    public string Nome { get; }
    public int Torque { get; }
    public float Relacao { get; }

    private MarchaEnum(string nome, int torque, float relacao)
    {
        Nome = nome;
        Torque = torque;
        Relacao = relacao;
    }

}