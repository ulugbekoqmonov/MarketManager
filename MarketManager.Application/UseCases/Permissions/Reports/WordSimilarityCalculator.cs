namespace MarketManager.Application.UseCases.Permissions.Reports;
public class WordSimilarityCalculator
{
    public static string CalculateSimilarity(string word)
    {
        double maxSimilarity = 0;
        string satr = "";
        string[] endPoints = { "user", "product", "permission", "supplier", "client", "expiredProduct", "role", "package", "paymentType", "productType", "order", "item" };
        for (int i = 0; i < endPoints.Length; i++)
        {
            int maxLength = Math.Max(word.Length, endPoints[i].Length);
            int distance = CalculateDistance(word, endPoints[i]);

            double minSimilarity = (1 - (double)distance / maxLength) * 100;
            if (minSimilarity > maxSimilarity)
            {
                maxSimilarity = minSimilarity;
                satr = endPoints[i];
            }
        }
        return satr;
    }

    private static int CalculateDistance(string word1, string word2)
    {
        int[,] dp = new int[word1.Length + 1, word2.Length + 1];

        for (int i = 0; i <= word1.Length; i++)
        {
            dp[i, 0] = i;
        }

        for (int j = 0; j <= word2.Length; j++)
        {
            dp[0, j] = j;
        }

        for (int i = 1; i <= word1.Length; i++)
        {
            for (int j = 1; j <= word2.Length; j++)
            {
                int cost = word1[i - 1] == word2[j - 1] ? 0 : 1;
                dp[i, j] = Math.Min(dp[i - 1, j - 1] + cost, Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1));
            }
        }

        return dp[word1.Length, word2.Length];
    }
}
