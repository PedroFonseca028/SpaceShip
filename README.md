cl# SpaceShip

Jogo 2D (Unity 6, URP) implementado conforme o enunciado "SpaceShip": parallax
scrolling do fundo + os itens do exercício (pontuação, tiro da nave, inimigos
com sprite próprio e desaceleração do tempo).

O projeto veio sem pasta `Assets` (nenhuma cena, sprite ou prefab). Como não é
possível abrir o Unity Editor neste ambiente para montar cena/prefabs à mão de
forma segura, e para não depender de baixar assets de terceiros, o jogo inteiro
foi implementado em C# em `Assets/Scripts/`, incluindo sprites gerados
proceduralmente em runtime (nave, inimigo, tiro, power-up e o fundo estrelado).
Assim ele roda 100% sozinho, sem precisar importar nada do Moodle antes de
testar — e pode ser aberto em qualquer máquina com o Unity 6000.0.82f1 (ou
compatível) instalado.

## Como rodar

1. Abra a pasta do projeto pelo Unity Hub (versão 6000.0.82f1 ou superior).
2. Crie uma cena vazia: `File > New Scene` (modelo "Basic (Built-in)" ou "2D (URP)" — tanto faz) e salve-a em `Assets/Scenes/Main.unity`.
3. Crie um GameObject vazio na cena (`GameObject > Create Empty`) e renomeie-o, por exemplo, para `Bootstrapper`.
4. Adicione o componente `GameBootstrapper` a esse GameObject (`Add Component > Game Bootstrapper`).
5. Aperte Play. A câmera, o fundo em parallax, a nave, os inimigos, o placar e a UI são montados automaticamente.

Controles: setas/WASD para mover, Ctrl esquerdo (ou botão esquerdo do mouse — ação `Fire1`) para atirar, `R` para reiniciar depois de perder.

## Onde cada requisito do enunciado foi implementado

- **Parallax Scrolling** (algoritmo do pintor, camadas com velocidades diferentes): [`Assets/Scripts/Background/Parallax.cs`](Assets/Scripts/Background/Parallax.cs) — mesma lógica do slide (`parallaxEffect` entre 0 e 1, reposiciona o sprite ao sair da tela), generalizada para funcionar com qualquer número de tiles por camada. Duas camadas (`FarStars`/`NearStars`) são montadas em [`GameBootstrapper.cs`](Assets/Scripts/Core/GameBootstrapper.cs).
- **Pontuação**: [`Assets/Scripts/Systems/ScoreManager.cs`](Assets/Scripts/Systems/ScoreManager.cs) + exibição em [`UIController.cs`](Assets/Scripts/UI/UIController.cs).
- **Nave atirar (só para frente)**: [`Assets/Scripts/Player/PlayerShooting.cs`](Assets/Scripts/Player/PlayerShooting.cs) + [`Bullet.cs`](Assets/Scripts/Combat/Bullet.cs).
- **Inimigos com sprite próprio**: [`Assets/Scripts/Combat/Enemy.cs`](Assets/Scripts/Combat/Enemy.cs) + [`EnemySpawner.cs`](Assets/Scripts/Combat/EnemySpawner.cs) (sprite distinto do da nave, gerado em [`SpriteFactory.cs`](Assets/Scripts/Visuals/SpriteFactory.cs)).
- **Desacelerar o tempo** (vantagem ao jogador, dispara por pontuação ou power-up): [`Assets/Scripts/Systems/SlowMotionController.cs`](Assets/Scripts/Systems/SlowMotionController.cs) + [`PowerUps/PowerUp.cs`](Assets/Scripts/PowerUps/PowerUp.cs). Só o fundo e os inimigos desaceleram (não usa `Time.timeScale` global) — a nave e os tiros continuam na velocidade normal, exatamente como pedido no enunciado.

## Trocando os sprites gerados pelos assets reais do Moodle

Os sprites são gerados em `Assets/Scripts/Visuals/SpriteFactory.cs` (métodos `CreateShipSprite`, `CreateEnemySprite`, `CreateBulletSprite`, `CreatePowerUpSprite`, `CreateStarLayerSprite`). Para usar os assets originais, basta trocar o corpo desses métodos por `Resources.Load<Sprite>("caminho-do-asset")` apontando para as imagens importadas em uma pasta `Assets/Resources`, sem precisar mexer em nenhum outro script.