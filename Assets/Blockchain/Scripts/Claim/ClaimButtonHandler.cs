using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DD.Web3
{
    public class ClaimButtonHandler : MonoBehaviour
    {
        public Button[] claimButtons;

        [SerializeField] private TextMeshProUGUI scoreText; // Assign your score Text here

        [SerializeField] private int claimAmount = 0;

        private void Start()
        {
            foreach (var claimButton in claimButtons)
            {
                claimButton.onClick.AddListener(OnClaimButtonClicked);
            }
        }

        private void OnClaimButtonClicked()
        {
            // Fetch claimAmount from scoreText
            if (scoreText != null && int.TryParse(scoreText.text, out claimAmount))
            {
                Debug.Log("Claim Amount (from Text) = " + claimAmount);
            }
            else
            {
                Debug.LogWarning("Score Text is null or not a valid number!");
                return;
            }

            if (BlockchainManager.Instance != null)
            {
                HandleClaimFlow();
            }
            else
            {
                Debug.Log("Blockchain or Wallet is not being used !");
            }
        }

        private void HandleClaimFlow()
        {
            BlockchainManager.Instance.connectionManager.ClaimDropERC20(claimAmount, (result) =>
            {
                if (result)
                {
                    OnTransactionSuccessful();
                }
                else
                {
                    OnTransactionFailed();
                }
            });
        }

        private void OnTransactionSuccessful()
        {
            Debug.Log("Transaction successful.");
            BlockchainManager.Instance.connectionManager.ShowLoadingScreen(false);
        }

        private void OnTransactionFailed()
        {
            Debug.Log("Transaction failed.");
            BlockchainManager.Instance.connectionManager.ShowLoadingScreen(false);
        }

        private void OnDestroy()
        {
            foreach (var claimButton in claimButtons)
            {
                claimButton.onClick.RemoveAllListeners();
            }
        }
    }
}
