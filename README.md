# UniCommon
[![npm version](https://badge.fury.io/js/unicommon.svg)](https://badge.fury.io/js/unicommon)  

---

Unityのユーティリティライブラリです。
[@keroxp](https://twitter.com/keroxp)が[黒羽のアリカ](http://hexat.studio/arika)を作ったときに開発したコードから依存関係のないものを取り出して再構築しました。

# Install

## npm/unity-package-syncerを使う方法

npmからインストールできます。  

`npm i unicommon --save`  

インストール後、[unity3d-package-syncer](https://github.com/rotorz/unity3d-package-syncer)を使ってnpmでこのプロジェクトをインポートします。  

`mkdir -p Assets/Plugins/Packages`  
`$(npm bin)/unity3d--sync`  

これで`Assets/Plugins/Packages/UniCommon`フォルダにライブラリがインポートされます。依存関係として`UniRx`も同ディレクトリにインポートされます。

# Usage
ランタイムでは、以下のコードをどこかで実行するだけで準備は完了です。
```
using UniCommon;
public class YouApp : MonoBehaviour {
    void Awake() {
      UniCommon.Initialize();
    }
}
```

# Contributing

バグ報告、修正依頼、その他便利機能などありましたらイシュー登録か、プルリクエストをください。コード規約などは特に設けていません。プルリクエストで修正します。
