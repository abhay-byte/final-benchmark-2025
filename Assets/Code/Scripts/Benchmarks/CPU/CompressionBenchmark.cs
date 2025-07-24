using UnityEngine;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

public class CompressionBenchmark : MonoBehaviour
{

    private CPUBenchmark cpuBenchmark;
    private CompressionBenchmarkData data;

    public string inputText = "This is a sample text to benchmark compression and encoding.";
    private byte[] inputData;
    private byte[] compressedData;
    private string base64EncodedData;

    private void Awake()
    {
        Utils utility = GetComponent<Utils>();
        data = new CompressionBenchmarkData();
        
        cpuBenchmark = utility.GetCPUBenchmarkScript();
    }

    public void CompressionBeginBenchmark()
    {
        inputData = Encoding.UTF8.GetBytes(inputText);

        Stopwatch stopwatch = new Stopwatch();

        // Compression Benchmark
        stopwatch.Start();
        compressedData = Compress(inputData);
        stopwatch.Stop();

        TimeSpan compressionTime = stopwatch.Elapsed;
        UnityEngine.Debug.Log($"Compression Time: {compressionTime.TotalMilliseconds} ms");
        data.compressionTime = compressionTime.TotalMilliseconds/1000;

        // Base64 Encoding Benchmark
        stopwatch.Reset();
        stopwatch.Start();
        base64EncodedData = Base64Encode(compressedData);
        stopwatch.Stop();

        TimeSpan encodingTime = stopwatch.Elapsed;
        UnityEngine.Debug.Log($"Base64 Encoding Time: {encodingTime.TotalMilliseconds} ms");
        data.base64EncodingTime = encodingTime.TotalMilliseconds/1000;

        // Base64 Decoding Benchmark
        byte[] decodedData;
        stopwatch.Reset();
        stopwatch.Start();
        decodedData = Base64Decode(base64EncodedData);
        stopwatch.Stop();

        TimeSpan decodingTime = stopwatch.Elapsed;
        UnityEngine.Debug.Log($"Base64 Decoding Time: {decodingTime.TotalMilliseconds} ms");
        data.base64DecordingTime = decodingTime.TotalMilliseconds/1000;

        // Decompression Benchmark
        byte[] decompressedData;
        stopwatch.Reset();
        stopwatch.Start();
        decompressedData = Decompress(decodedData);
        stopwatch.Stop();

        TimeSpan decompressionTime = stopwatch.Elapsed;
        UnityEngine.Debug.Log($"Decompression Time: {decompressionTime.TotalMilliseconds} ms");
        data.decompressionTime = decompressionTime.TotalMilliseconds/1000;

        // Verify the decompressed data matches the original input
        string decompressedText = Encoding.UTF8.GetString(decompressedData);
        if (inputText == decompressedText)
        {
            UnityEngine.Debug.Log("Decompressed data matches original input.");
        }
        else
        {
            UnityEngine.Debug.LogError("Decompressed data does not match original input.");
        }

        SetCompressionBenchmarkResult();
        cpuBenchmark.BeginBenchamrk();
    }

    private byte[] Compress(byte[] data)
    {
        using (MemoryStream compressedStream = new MemoryStream())
        {
            using (DeflateStream compressionStream = new DeflateStream(compressedStream, CompressionMode.Compress))
            {
                compressionStream.Write(data, 0, data.Length);
            }
            return compressedStream.ToArray();
        }
    }

    private byte[] Decompress(byte[] data)
    {
        using (MemoryStream compressedStream = new MemoryStream(data))
        {
            using (MemoryStream decompressedStream = new MemoryStream())
            {
                using (DeflateStream decompressionStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(decompressedStream);
                }
                return decompressedStream.ToArray();
            }
        }
    }

    private string Base64Encode(byte[] data)
    {
        return Convert.ToBase64String(data);
    }

    private byte[] Base64Decode(string base64)
    {
        return Convert.FromBase64String(base64);
    }

    private void SetCompressionBenchmarkResult()
    {
        CPUBenchmarkData benchmarkData = cpuBenchmark.benchmarkData;
        benchmarkData.compressionBenchmarkdata = data;
    }
}

public class CompressionBenchmarkData
{
    public double compressionTime;
    public double base64EncodingTime;
    public double base64DecordingTime;
    public double decompressionTime;
}