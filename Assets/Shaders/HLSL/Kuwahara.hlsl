TEXTURE2D_X(_BlitTexture);

float3 SampleCol(float2 uv)
{
    return SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv).rgb;
}

void Kuwahara_float(
    float2 UV,
    float Radius,
    float Intensity, // NEW: contrôle style anime
    out float3 Color)
{
    float2 texelSize = 1.0 / _ScreenParams.xy;
    int r = (int) Radius;

    float3 mean[4];
    float3 var[4];
    float count[4];

    [unroll]
    for (int k = 0; k < 4; k++)
    {
        mean[k] = 0;
        var[k] = 0;
        count[k] = 0;
    }

    // =========================
    // 4 quadrants Kuwahara
    // =========================

    for (int y = -r; y <= 0; y++)
        for (int x = -r; x <= 0; x++)
        {
            float3 c = SampleCol(UV + float2(x, y) * texelSize);

            mean[0] += c;
            var[0] += c * c;
            count[0]++;
        }

    for (int y = -r; y <= 0; y++)
        for (int x = 0; x <= r; x++)
        {
            float3 c = SampleCol(UV + float2(x, y) * texelSize);

            mean[1] += c;
            var[1] += c * c;
            count[1]++;
        }

    for (int y = 0; y <= r; y++)
        for (int x = -r; x <= 0; x++)
        {
            float3 c = SampleCol(UV + float2(x, y) * texelSize);

            mean[2] += c;
            var[2] += c * c;
            count[2]++;
        }

    for (int y = 0; y <= r; y++)
        for (int x = 0; x <= r; x++)
        {
            float3 c = SampleCol(UV + float2(x, y) * texelSize);

            mean[3] += c;
            var[3] += c * c;
            count[3]++;
        }

    // =========================
    // selection style anime
    // =========================

    float minVar = 1e20;
    float3 result = 0;

    float3 center = SampleCol(UV);

    for (int k = 0; k < 4; k++)
    {
        mean[k] /= count[k];
        var[k] = (var[k] / count[k]) - (mean[k] * mean[k]);

        float variance = dot(var[k], 1.0);

        // Anime bias : favorise homogénéité + stabilité
        variance = sqrt(variance);

        // évite flickering + stabilise zones plates
        variance += length(mean[k] - center) * 0.25;

        if (variance < minVar)
        {
            minVar = variance;
            result = mean[k];
        }
    }

    // =========================
    // ANIME FINISHING PASS
    // =========================

    // légère interpolation vers centre (style illustration)
    result = lerp(result, center, Intensity * 0.15);

    // contraste doux (look anime)
    result = pow(result, 1.0 / (1.0 + Intensity * 0.2));

    Color = result;
}