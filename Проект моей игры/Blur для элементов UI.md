Чтобы создать эффект размытия фона (blur) в Unity UI, вам нужно использовать специальный подход с Canvas и эффектами постобработки. Вот шаги, которые помогут вам это реализовать:

1. **Создайте Canvas**: Если у вас еще нет Canvas, создайте его, щелкнув правой кнопкой мыши в Hierarchy и выбрав `UI -> Canvas`.
    
2. **Добавьте Image для фона**: Добавьте элемент `UI -> Image` в Canvas, который будет служить фоном.
    
3. **Подготовьте Shader для размытия**: Вам понадобится специальный Shader для размытия. Создайте новый Shader:
    
    - Щелкните правой кнопкой мыши в Project и выберите `Create -> Shader -> Unlit Shader`.
    - Назовите его, например, `BlurShader`.
    - Замените содержимое следующим кодом:
```
Shader "Custom/BlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurAmount ("Blur Amount", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _BlurAmount;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                for (int x = -_BlurAmount; x <= _BlurAmount; x++)
                {
                    for (int y = -_BlurAmount; y <= _BlurAmount; y++)
                    {
                        col += tex2D(_MainTex, i.uv + float2(x, y) * 0.001);
                    }
                }
                return col / (1 + (_BlurAmount * 2) * (_BlurAmount * 2)); // нормализация
            }
            ENDCG
        }
    }
}
```
4. **Создайте Material**: Создайте Material, используя этот Shader:
    
    - Щелкните правой кнопкой мыши в Project и выберите `Create -> Material`.
    - Назовите его, например, `BlurMaterial`.
    - Выберите созданный Shader в этом Material.
5. **Примените Material к Image**: Перетащите `BlurMaterial` на ваш `Image`, который вы создали на шаге 2.
    
6. **Настройка Canvas**: Убедитесь, что Canvas настроен на режим рендеринга `Screen Space - Overlay`.
    

Теперь, когда вы запустите сцену, ваш фон будет размыт с помощью указанного эффекта. Вы можете регулировать уровень размытия, изменяя значение `Blur Amount` в `BlurMaterial`.

Если вам нужно больше контроля или более сложные эффекты размытия, вы можете рассмотреть использование системы постобработки Unity или более продвинутые шейдеры.
