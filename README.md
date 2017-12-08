# UniCommon
ゆにこもん

---

Unityのユーティリティライブラリです。
[@keroxp](https://twitter.com/keroxp)が[黒羽のアリカ](http://hexat.studio/arika)を作ったときに開発したコードから依存関係のないものを取り出して再構築しました。

# How To Use

## UniRxを追加する

UniRxに依存しているので、Asset StoreからUniRxをインポートしておきます。

## unity-package-syncerを使う方法
[https://github.com/rotorz/unity3d-package-syncer](https://github.com/rotorz/unity3d-package-syncer)を使ってnpmでこのプロジェクトをインポート出来ます。

`npm i unity-package-syncer --save`  
`npm i git+https://github.com:keroxp/UniCommon.git#master --save`  
`mkdir -p Assets/Plugins/Packages`  
`$(npm bin)/unity3d--sync`  


これで`Assets/Plugins/Packages/UniCommon`フォルダにライブラリがインポートされます。

## Zipをダウンロードする方法

右上からzipをダウンロードしてプロジェクトに追加してください。（おすすめしません）
