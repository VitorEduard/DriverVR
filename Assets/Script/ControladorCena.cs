using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorCena : MonoBehaviour
{
    // Troca para uma cena especificada pelo nome
    public void TrocarCena(string nomeCena)
    {
        SceneManager.LoadScene(nomeCena);
    }

    // Encerra o jogo
    public void FecharJogo()
    {
        #if UNITY_EDITOR
        // Fecha o jogo dentro do Editor da Unity
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Fecha o jogo na build final
            Application.Quit();
        #endif
    }
}
