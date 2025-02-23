# Proyecto PMDM RollABall

## Descripción

Este proyecto es una implementación de un juego de tipo *Roll-a-ball* en Unity, donde se controla a una esfera que puede moverse en un entorno 3D y recoger objetos mientras escapa de enemigos. El juego también incluye dos tipos de cámaras: una en tercera persona y otra en primera persona, que el jugador puede alternar. El objetivo es familiarizarse con el uso de *Rigidbody*, control de entrada del teclado, y manejo de cámaras en Unity.

A mayores realice un objeto que al objtenerlo consigues la opción de realizar un salto, creación de un mapa, una pared invisible, una rampa para inpulsarte y que cuando los enemigos te tocan se reinicia el nivel.

## Primer Script:  CamaraController

Este proyecto implementa un controlador de cámara en Unity que permite alternar entre vistas en **primera persona** y **tercera persona**. Proporciona una experiencia inmersiva para juegos en los que el jugador controla un objeto (en este caso, una pelota).

## Características

- **Vista en tercera persona**:
  - La cámara sigue al jugador manteniendo una distancia definida.
  - La cámara enfoca constantemente al jugador desde atrás.

- **Vista en primera persona**:
  - La cámara se coloca sobre el jugador, simulando su perspectiva.
  - La rotación de la cámara se controla con el ratón.
  - Movimiento físico del jugador basado en la orientación de la cámara.

- **Intercambio dinámico de vistas**:
  - Pulsar `1` cambia a la vista en tercera persona.
  - Pulsar `2` cambia a la vista en primera persona.

## Requisitos

- **Motor de juego**: Unity 2020.3 o superior.
- **Componente necesario**: 
  - Un objeto con un `Rigidbody` (el jugador).
  - Un objeto de cámara principal asociado al script.

## Cómo funciona

### Variables configurables
En el script se pueden ajustar los siguientes parámetros:
- **`mouseSensitivity`**: Controla la sensibilidad del ratón para la rotación en primera persona.
- **`heightOffset`**: Define la altura de la cámara en relación al jugador en primera persona.
- **`distanceFromPlayer`**: Determina la distancia de la cámara al jugador en tercera persona.
- **`movementSpeed`**: Ajusta la velocidad del movimiento del jugador.

### Uso de las teclas
- **Tecla `1`**: Cambia a la vista en tercera persona.
- **Tecla `2`**: Cambia a la vista en primera persona.

### Configuración inicial
- La posición y rotación de la cámara en tercera persona se guardan al inicio.
- La cámara en primera persona bloquea el cursor para evitar interferencias.

## Cómo usarlo

1. **Añadir el script**:
   - Asocia el script `CameraController` al objeto de cámara principal.

2. **Asignar referencias**:
   - Arrastra el objeto jugador al campo `player`.
   - Configura el `Transform` del jugador en `playerBody`.

3. **Personalizar las configuraciones**:
   - Ajusta los valores de sensibilidad, distancia, y velocidad según las necesidades del juego.

4. **Ejecutar el juego**:
   - Usa las teclas `1` y `2` para alternar entre vistas mientras controlas al jugador.

## Código del script

El script completo está disponible en el archivo `CameraController.cs`.

## Segundo Script: PlayerController

Este script implementa un controlador de movimiento para un jugador en Unity, utilizando el componente `Rigidbody` para aplicar física en función de las entradas del usuario.

## Características

- **Movimiento físico**:
  - Basado en las entradas de teclado o gamepad.
  - La velocidad del movimiento es ajustable mediante un parámetro público.

- **Compatibilidad con Unity Input System**:
  - Usa el sistema de entrada moderno de Unity (`UnityEngine.InputSystem`).

- **Física realista**:
  - El movimiento se gestiona con `Rigidbody` y fuerzas físicas para mayor realismo.

## Requisitos

- **Motor de juego**: Unity 2020.3 o superior.
- **Componente necesario**:
  - Un objeto con un `Rigidbody` adjunto.
  - Configuración del nuevo sistema de entrada de Unity (Input System).

## Cómo funciona

### Métodos principales

1. **`Start()`**:
   - Obtiene y almacena el componente `Rigidbody` del jugador para aplicarle fuerzas más adelante.

2. **`OnMove(InputValue movementValue)`**:
   - Este método captura las entradas de movimiento en los ejes X e Y (por ejemplo, de un joystick o las teclas WASD) y las almacena en las variables `movementX` y `movementY`.

3. **`FixedUpdate()`**:
   - Calcula un vector de movimiento 3D usando las entradas X e Y.
   - Aplica una fuerza al `Rigidbody` del jugador usando este vector y el valor de velocidad (`speed`).

### Variables configurables

- **`speed`**: Controla la velocidad a la que se mueve el jugador. Puedes modificar este valor en el editor de Unity.

### Física y movimiento
El movimiento es gestionado mediante el método `AddForce`, lo que garantiza un comportamiento físico suave y permite interacciones con otros objetos físicos en la escena.

---

## Cómo usarlo

1. **Preparar el jugador**:
   - Añade un objeto 3D (como una esfera o un cubo) a la escena.
   - Asegúrate de que el objeto tiene un componente `Rigidbody` adjunto.

2. **Añadir el script**:
   - Asocia el script `PlayerController` al objeto del jugador.

3. **Configurar el sistema de entrada**:
   - Configura un esquema de entrada compatible con el sistema Input System.
   - Asegúrate de que las acciones de movimiento están asignadas correctamente (por ejemplo, `Horizontal` y `Vertical`).

4. **Personalizar la velocidad**:
   - Ajusta el valor de `speed` en el inspector de Unity según sea necesario.

5. **Ejecutar el juego**:
   - Controla el jugador con las teclas WASD, un joystick o cualquier dispositivo configurado en el Input System.

---

## Código del script 

El script completo está disponible en el archivo `PlayerController.cs`.


## Tercer Script: Mouselook

Este script implementa el control de rotación de la cámara en Unity utilizando las entradas del ratón. Permite al jugador mirar alrededor en un entorno 3D al mover el ratón, con restricciones para evitar rotaciones no naturales.

## Características

- **Control de cámara basado en ratón**:
  - Movimiento horizontal (eje X del ratón): Rota el cuerpo del jugador.
  - Movimiento vertical (eje Y del ratón): Controla la inclinación de la cámara.

- **Limitación de la vista vertical**:
  - La rotación en el eje X está limitada para evitar que el jugador gire completamente hacia atrás.

- **Cursor bloqueado**:
  - El cursor se oculta y se fija al centro de la pantalla para una experiencia de control inmersiva.

## Requisitos

- **Motor de juego**: Unity 2020.3 o superior.
- **Componente necesario**:
  - Una cámara principal que tenga este script adjunto.
  - Un objeto jugador con un `Transform` asignado como referencia para la rotación horizontal.

## Cómo funciona

### Métodos principales

1. **`Start()`**:
   - Configura el cursor para que esté bloqueado en el centro de la pantalla y no sea visible.

2. **`Update()`**:
   - Captura los movimientos del ratón en los ejes X e Y, ajustados por un parámetro de sensibilidad y el tiempo entre fotogramas (`Time.deltaTime`).
   - **Movimiento vertical**:
     - Acumula la rotación en el eje X.
     - Limita el ángulo de inclinación vertical entre -90° y 90° con `Mathf.Clamp`.
     - Aplica la rotación al componente `Transform` de la cámara.
   - **Movimiento horizontal**:
     - Rota el cuerpo del jugador (`playerBody`) en el eje Y para simular el giro horizontal.

### Variables configurables

- **`Sensibilidad`**:
  - Controla la rapidez de la rotación al mover el ratón.
  - Se puede ajustar en el inspector de Unity.

- **`playerBody`**:
  - Referencia al objeto que representa el cuerpo del jugador, utilizado para aplicar la rotación horizontal.

- **`xRotacion`**:
  - Almacena la rotación acumulada en el eje X para mantener el seguimiento de la inclinación vertical.

## Cómo usarlo

1. **Añadir el script**:
   - Asocia este script a la cámara principal de la escena.

2. **Configurar referencias**:
   - Arrastra el objeto del jugador al campo `playerBody` en el inspector de Unity.

3. **Ajustar sensibilidad**:
   - Modifica el valor de `Sensibilidad` en el inspector para un control más preciso o rápido según sea necesario.

4. **Ejecutar el juego**:
   - Mueve el ratón para controlar la cámara y la orientación del jugador.
   - Observa cómo la vista vertical está limitada y el cuerpo del jugador rota de manera horizontal.

---

## Código del script

El script completo está disponible en el archivo `Mouselook.cs`.

¡Gracias por usar este controlador de cámara! 🎮
