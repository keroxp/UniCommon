# UniCommon
ゆにこもん

---

Unityのユーティリティライブラリです。
[@keroxp](https://twitter.com/keroxp)が[黒羽のアリカ](http://hexat.studio/arika)を作ったときに開発したコードから依存関係のないものを取り出して再構築しました。

# How To Use

## unity-package-syncerを使う方法
[https://github.com/rotorz/unity3d-package-syncer](https://github.com/rotorz/unity3d-package-syncer)を使ってnpmでこのプロジェクトをインポート出来ます。

`npm i unity-package-syncer --save`  
`npm i git+https://github.com:keroxp/UniCommon.git#master --save`  
`mkdir -p Assets/Plugins/Packages`  
`$(npm bin)/unity3d--sync`  


これで`Assets/Plugins/Packages/UniCommon`フォルダにライブラリがインポートされます。依存関係として`UniRx`も同ディレクトリにインポートされます。

## Zipをダウンロードする方法

右上からzipをダウンロードしてプロジェクトに追加してください。その場合アセットストアからUniRxをインポートする必要がありますが、2017/12/09現在、アセットストアのUniRx（5.6.0）がUnity2017.2/.Net 4.6環境に対応していないので、UniCommonもコンパイルできなくなります。

** なので、おすすめしません😫 **
