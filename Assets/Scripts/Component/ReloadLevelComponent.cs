using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WhereIAm.Scripts.Component
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var scene = SceneManager.GetActiveScene();

            SceneManager.LoadScene(scene.name);
        }
    }
}