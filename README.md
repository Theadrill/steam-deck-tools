# (Windows) Steam Deck Tools - Custom Fan Curve Fork

[![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/ayufan/steam-deck-tools?label=stable&style=flat-square)](https://github.com/ayufan/steam-deck-tools/releases/latest)
[![GitHub release (latest SemVer including pre-releases)](https://img.shields.io/github/v/release/ayufan/steam-deck-tools?color=red&include_prereleases&label=beta&style=flat-square)](https://github.com/ayufan/steam-deck-tools/releases)
![GitHub all releases](https://img.shields.io/github/downloads/ayufan/steam-deck-tools/total?style=flat-square)

> **Fork with Editable Custom Fan Curve Support**  
> This fork adds a new customizable `Custom` fan profile that reads user-defined fan curve points dynamically from `FanControl.dll.ini` with real-time hot-reloading, while keeping the official Valve `SteamOS` fan curve 100% untouched.

<img src="docs/images/overlay.png" height="400"/>

---

## English: Custom Fan Curve Feature

### Why this fork was created
Steam Deck Tools originally includes four fan modes: `Default`, `Silent`, `SteamOS`, and `Max`.
* **The Stock SteamOS Curve Limitation:** The default SteamOS curve was engineered by Valve primarily for quiet acoustics (reaching only ~3,450 RPM at 70°C and ~5,000 RPM at 80°C). When running Windows—especially for users with unlocked BIOS (e.g., Smokeless UMAF) operating at higher TDPs (18W–25W)—this conservative curve allows heat to build up rapidly, causing thermal saturation and aggressive performance throttling.
* **The Max Profile Limitation:** The `Max` profile forces 100% full speed (7,300 RPM) at all times, which is unnecessarily noisy when idling or running lighter games.
* **The Solution:** This fork introduces the **`Custom`** profile. It provides proactive, aggressive cooling where you need it, interpolates smoothly between temperature thresholds, and can be edited anytime without recompiling the project.
* **Compatibility:** 
  * The stock **`SteamOS`** profile remains completely untouched and functional with Valve's original quadratic curve.
  * The new **`Custom`** profile is fully integrated into both the Windows System Tray menu and the in-game RTSS Quick Access Menu (the `...` button).

### How to Edit Your Fan Curve
1. Open your Steam Deck Tools installation folder (default: `C:\Program Files\SteamDeckTools\`).
2. Open `FanControl.dll.ini` in any text editor (such as Notepad).
3. Under the `[CustomCurve]` section, configure your temperature:RPM pairs on the `Curve` line:
   ```ini
   [CustomCurve]
   Curve=50:2200, 55:3200, 60:4200, 65:5200, 70:6200, 75:6800, 80:7300
   ```
4. **Default Aggressive Scale:**
   * **Below 50°C:** 2,200 RPM (quiet for desktop navigation and menus)
   * **55°C:** 3,200 RPM (early ramp-up before the copper heatsink saturates)
   * **60°C:** 4,200 RPM (solid cooling for lighter titles)
   * **65°C:** 5,200 RPM (strong airflow for moderate TDP)
   * **70°C:** 6,200 RPM (heavy cooling to stall temperature rise)
   * **75°C:** 6,800 RPM (near-maximum cooling to sustain APU boost)
   * **80°C+:** 7,300 RPM (Steam Deck EC hardware maximum ceiling)
5. **Real-Time Hot Reload:** As soon as you save the file (Ctrl + S), `FanControl` detects the file modification and applies your new curve immediately—no restart required!
6. **Hardware Protections:** Internal safety thresholds (EC hardware 7,300 RPM clamp, APU minimum idle floor, SSD and battery temperature safeties) remain fully active.

---

## Português: Recurso de Curva Customizada de Ventoinha

### Por que este fork foi criado
O Steam Deck Tools oficial possui quatro modos de ventoinha: `Default`, `Silent`, `SteamOS` e `Max`.
* **A limitação da curva original do SteamOS:** A curva padrão da Valve foi projetada com foco quase que exclusivo no silêncio (atinge apenas ~3.450 RPM a 70°C e ~5.000 RPM a 80°C). No Windows—especialmente para quem utiliza BIOS desbloqueada (como Smokeless UMAF) e TDP aumentado para 18W–25W—o pequeno dissipador do Steam Deck satura em poucos segundos, gerando perda de FPS por *thermal throttling*.
* **A limitação do perfil Max:** O modo `Max` trava o ventilador em 100% direto (7.300 RPM), sendo barulhento desnecessariamente em momentos de repouso ou jogos leves.
* **A solução:** Este fork adiciona o perfil **`Custom`**. Ele permite uma curva de alta performance com transições suaves e totalmente configurável através de um arquivo `.ini`.
* **Compatibilidade Total:**
  * O perfil **`SteamOS`** original da Valve continua **100% intacto** para quem desejar usá-lo.
  * O novo perfil **`Custom`** aparece nativamente tanto no menu da bandeja do Windows (ícone do ventilador) quanto no menu rápido do RTSS dentro dos jogos (botão `...`).

### Como editar a sua curva de ventoinha
1. Abra a pasta de instalação do Steam Deck Tools (padrão: `C:\Program Files\SteamDeckTools\`).
2. Abra o arquivo `FanControl.dll.ini` no Bloco de Notas.
3. Na seção `[CustomCurve]`, configure os pontos de `temperatura:RPM` na linha `Curve`:
   ```ini
   [CustomCurve]
   Curve=50:2200, 55:3200, 60:4200, 65:5200, 70:6200, 75:6800, 80:7300
   ```
4. **Escala Otimizada Padrão:**
   * **Abaixo de 50°C:** 2.200 RPM (silencioso para uso básico no Windows)
   * **55°C:** 3.200 RPM (resfriamento preventivo)
   * **60°C:** 4.200 RPM (dissipação eficiente para jogos médios)
   * **65°C:** 5.200 RPM (forte fluxo de ar para 12W–15W)
   * **70°C:** 6.200 RPM (pressão pesada contra avanço térmico)
   * **75°C:** 6.800 RPM (prepara para segurar picos de TDP)
   * **80°C ou mais:** 7.300 RPM (teto físico da controladora de hardware do Steam Deck)
5. **Hot-Reload em Tempo Real:** Ao salvar o arquivo (Ctrl + S), o `FanControl` detecta a alteração automaticamente no segundo seguinte e atualiza a rotação—sem necessidade de reiniciar o aplicativo!
6. **Proteções de Hardware:** Todos os limites de segurança da placa (corte em 7.300 RPM, rotação mínima de segurança da APU e proteções térmicas para SSD e bateria) permanecem ativos.

---

## Acknowledgements & Credits / Créditos e Agradecimentos

* All core credit goes to **Kamil Trzciński ([@ayufan](https://github.com/ayufan))**, the original author of the fantastic **[Steam Deck Tools](https://github.com/ayufan/steam-deck-tools)** project.
* Original repository: [https://github.com/ayufan/steam-deck-tools](https://github.com/ayufan/steam-deck-tools)
* Website & Documentation: [https://steam-deck-tools.ayufan.dev/](https://steam-deck-tools.ayufan.dev/)
* If you enjoy Steam Deck Tools, consider supporting Kamil on [Ko-fi](https://ko-fi.com/ayufan) or [PayPal](https://www.paypal.com/donate/?hosted_button_id=DHNBE2YR9D5Y2).

---

## Upstream Project Overview

This repository contains personal tools to help running Windows on Steam Deck.

**This software is provided on best-effort basis and can break your SteamDeck.**

### Applications

This project provides the following applications:

- [Fan Control](https://steam-deck-tools.ayufan.dev/fan-control) - control Fan on Windows
- [Performance Overlay](https://steam-deck-tools.ayufan.dev/performance-overlay) - see FPS and other stats
- [Power Control](https://steam-deck-tools.ayufan.dev/power-control) - change TDP or refresh rate
- [Steam Controller](https://steam-deck-tools.ayufan.dev/steam-controller) - use Steam Deck with Game Pass

### Additional Information

- [Controller Shortcuts](https://steam-deck-tools.ayufan.dev/shortcuts) - default shortcuts when using [Steam Controller](https://steam-deck-tools.ayufan.dev/steam-controller).
- [Development](https://steam-deck-tools.ayufan.dev/development) - how to compile this project.
- [Risks](https://steam-deck-tools.ayufan.dev/risks) - this project uses kernel manipulation and might result in unstable system.
- [Privacy](https://steam-deck-tools.ayufan.dev/privacy) - this project can connect to remote server to check for auto-updates or track errors
- [Troubleshooting](https://steam-deck-tools.ayufan.dev/troubleshooting) - if you encounter any problems.

### Join Us

Join Us for help or chat. We are at [Official WindowsOnDeck](https://discord.gg/uF7kd33u7u) Discord server.

### Anti-Cheat and Antivirus software

[READ IF PLAYING ONLINE GAMES AND/OR GAMES THAT HAVE ANTI-CHEAT ENABLED](https://steam-deck-tools.ayufan.dev/#anti-cheat-and-antivirus-software).

### Author

Kamil Trzciński, 2022-2024

Steam Deck Tools is not affiliated with Valve, Steam, or any of their partners.

### License

[Creative Commons Attribution-NonCommercial-ShareAlike (CC-BY-NC-SA)](http://creativecommons.org/licenses/by-nc-sa/4.0/).

Free for personal use. Contact Kamil in other cases (`ayufan@ayufan.eu`).
