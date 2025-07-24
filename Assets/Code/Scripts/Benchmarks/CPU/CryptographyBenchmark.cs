using UnityEngine;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

public class CryptographicBenchmark : MonoBehaviour
{
    private CPUBenchmark cpuBenchmark;
    private CryptographicBenchmarkData data;

    public int numIterations = 100000; // Adjust the number of iterations for benchmarking
    public string plainText = "Hello, Unity!"; // Text to be encrypted

    private byte[] key;
    private byte[] iv;
    private TripleDESCryptoServiceProvider desProvider;
    private Stopwatch stopwatch = new Stopwatch();

    private void Awake()
    {
        data = new CryptographicBenchmarkData();
        Utils utility = GetComponent<Utils>();
        cpuBenchmark = utility.GetCPUBenchmarkScript();
    }

    public void BeginCryptographicBenchmark()
    {
        InitializeDES();

        // Encrypt and decrypt the text repeatedly to measure performance
        BenchmarkEncryption();
        BenchmarkDecryption();
    }

    private void InitializeDES()
    {
        // Generate a random 3DES key and IV
        desProvider = new TripleDESCryptoServiceProvider();
        desProvider.GenerateKey();
        desProvider.GenerateIV();
        key = desProvider.Key;
        iv = desProvider.IV;
    }

    private void BenchmarkEncryption()
    {
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);

        stopwatch.Start();
        for (int i = 0; i < numIterations; i++)
        {
            using (var desEncryptor = desProvider.CreateEncryptor(key, iv))
            {
                byte[] encryptedBytes = desEncryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);
            }
        }
        stopwatch.Stop();

        double elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
        UnityEngine.Debug.Log($"3DES/CTR Encryption Benchmark - Iterations: {numIterations}");
        UnityEngine.Debug.Log($"Total Execution Time: {elapsedTime} ms");
        data.encryptionTime = elapsedTime/1000;
    }


    private void BenchmarkDecryption()
    {
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);

        // Encrypt the text once for benchmarking decryption
        byte[] encryptedBytes;
        using (var desEncryptor = desProvider.CreateEncryptor(key, iv))
        {
            encryptedBytes = desEncryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);
        }

        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < numIterations; i++)
        {
            using (var desDecryptor = desProvider.CreateDecryptor(key, iv))
            {
                byte[] decryptedBytes = desDecryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            }
        }
        stopwatch.Stop();

        double elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
        UnityEngine.Debug.Log($"3DES/CTR Decryption Benchmark - Iterations: {numIterations}");
        UnityEngine.Debug.Log($"Total Execution Time: {elapsedTime} ms");
        data.decryptionTime = elapsedTime/1000;

        SetCryptographicBenchmarkResult();
        cpuBenchmark.BeginBenchamrk();
    }
    private void SetCryptographicBenchmarkResult()
    {
        CPUBenchmarkData benchmarkData = cpuBenchmark.benchmarkData;
        benchmarkData.cryptographicBenchmarkData = data;
    }

}

public class CryptographicBenchmarkData
{
    public double encryptionTime;
    public double decryptionTime;
}