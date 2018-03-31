# HoloGPSReceiver
<img src="_github/demo.jpg" width="500">

# Features

* スマホから受けとったGPS・コンパス情報から自己位置推定を行い、あらかじめ設定した座標（観光地・ランドマークなど）との位置関係から情報提示を行う
    * ランドマークに近づいた際に音声でユーザーに通知し、オブジェクトを出現させる
    * ランドマークの撮影画像の表示
    * 遠方にあるランドマークの位置をUIとして表示する
    * 地図の表示

# How to setup
## 必要なもの

* HoloLens
* Unity 2017.2.2f1
* Visual Studio 2017
    * UWP開発パッケージのインストールが必要
* `Zig Sim`のインストールされたシムの入ったスマートフォン
    * `Zig Sim`はApp Store/Google Playから入手可

## 手順
### HoloLensへのデプロイ

1. masterブランチをCloneする。
1. Unityで開いて、Project Settingを変更する。
    1. メニューから`HoloToolkit`>`Configure`>`Apply HoloLens Project Settings`と進み、そのまま`Apply`を押す。
    1. メニューから`HoloToolkit`>`Configure`>`Apply HoloLens Capability Settings`と進み、`Internet Client Server` `Private Network Client Server`にチェックが入っていることを確認して`Apply`を押す。
1. `Build Settings`から`Build`し、指定したディレクトリに生成されたVSプロジェクトからHoloLensにデプロイする。
    1. 生成されたslnファイルをVSで開き、ビルド構成を`Release`、プラットフォームを`x86`とする。
    1. ビルド対象を`リモートコンピュータ`として、開いた設定画面からPCと同じLANに接続したHoloLensのIPAddressを入力する。HoloLensのIPAddressは`Setting`アプリの`Network`>`Advanced`から確認できる。
    1. メニューから`デバッグ`>`デバッグなしで開始`でデプロイする。
    1. 初回デプロイ時にはPINコードを要求されるので`Setting`アプリから`Security`> `Developer`と進み、`Pair device`を押した時に表示される六桁の数字を入力する。

### Zig Simアプリの設定
1. スマートフォンがHoloLensと同じLANに接続していることを確認する。
1. Zig Simアプリを起動し、
    1. `Sensor`タブを選択し、`Compass`と`GPS`のトグルをONにする。
    1. `Settings`タブを選択し、下の画像のように設定を変更する。
    <img src="_github/zigsimsettings.jpg" width="300">
1. HoloLens側のアプリの起動前もしくは起動後に`Start`タブを選択することでセンサ情報の送信が開始される。
