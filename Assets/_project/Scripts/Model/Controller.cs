using Model.Unit;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Model
{
    public class Controller : MonoBehaviour
    {
        [Header("Units")] [SerializeField] private Unit.Controller player;
        [SerializeField] private Unit.Controller enemy;
        [Header("Screens")] [SerializeField] private UI.Game.Controller ui;

        [Space(5)] [Header("Stats")] [SerializeField]
        private int hpPlayer;

        [SerializeField] private int hpEnemy;

        [Space(5)] [Header("Inventory")] [SerializeField]
        private Inventory inventory;

        [Space(5)] [SerializeField] private UIHandler uiHandler;


        private void Start()
        {
            uiHandler.Initialize(ui, inventory);
            uiHandler.TakePlayerDamageEvent += TakePlayerDamage;
            uiHandler.TakeEnemyDamageEvent += TakeEnemyDamage;
            uiHandler.SetBodyProtectionEvent += player.SetBody;
            uiHandler.SetHeadProtectionEvent += player.SetHead;
            uiHandler.TakePlayerReplishHealEvent += player.Heal.ReplenishHealth;
            player.Heal.OnHealthChangeEvent += ui.Player.HP.SetValue;
            enemy.Heal.OnHealthChangeEvent += ui.Enemy.HP.SetValue;

            player.Initialize(hpPlayer);
            enemy.Initialize(hpEnemy);

            enemy.DeathEvent += EnemyDeath;
            player.DeathEvent += PlayerDeath;
        }

        private void EnemyDeath()
        {
            uiHandler.StartGenerateItem();
            enemy.Initialize(hpEnemy);
            player.Initialize(hpPlayer);
        }

        private void PlayerDeath()
        {
            ui.GameOverPanel();
        }

        private void TakePlayerDamage()
        {
            player.Damage(Random.Range(15, 20));
        }

        private void TakeEnemyDamage(int value)
        {
            enemy.Damage(value);
        }
        public void Restart() => 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}