# Catpuccino

**Desarrolladores - Grupo B:**
- **Garazi Blanco Jauregi** → g.blancoj.2022@alumnos.urjc.es
- **Alberto López García de Ceca** → a.lopezgar.2022@alumnos.urjc.es
- **Pablo García García** → p.garciag.2022@alumnos.urjc.es
- **Cristina González de Lope** → c.gonzalezde.2022@alumnos.urjc.es
- **Ángela Sánchez Díaz** → a.sanchezdi.2022@alumnos.urjc.es
- **Laura de la Cruz Cañadas** → l.delacruz.2022@alumnos.urjc.es

**Redes sociales** → https://linktr.ee/mangostudiourjc?utm_source=linktree_profile_share&ltsid=bdd4e42b-4bf8-4e49-92eb-5ce8b950b732

## Version 1.0 - Fase Beta
---
## 0. Diferencias con la fase anterior
- Tutorial ingame implementado para los 3 primeros días.
- Página web actualizada.
- 5 clientes (concept, modedelado 3D y 2D)
- Gato (modelado 3D y animación de caminar)
- Nuevo contenido en RRSS.
- Puntación media en pantalla final.
- Mecánicas básicas actualizadas (pedidos para llevar).
- Desbloqueo de cafés y comidas por día (en total 7 días).
- Implementación de mensaje diario de elementos desbloqueados.
- Implementación de una pizarra desplegable en la que comprobar la carta disponible del día.
- Nuevas mecánicas de cafés implementadas: tipos de cafés (11), crema, leche condensada, whiskey, chocolate, leche, calentar leche...
- Nuevas mecánicas de comida implementadas: bizcochos, galletas, mufflins, horno...
- Diseño e implementación de todas las interfaces y botones.
- Implementación de una bandeja persistente entre panel de preparación de cafés y de comidas.
- Implementación de comprobar los requisitos de la orden mediante una nota desplegable en ambos paneles.
- Aumentado la complicación ingame e implementación de sistema de facturas que aumenta gradualmente con los días.
- El tiempo ingame disminuye de forma más lenta con el transcurso de los días.
- Diseñados e implementados los "*props*" del juego (todos los tipos de cafés, comidas, maquinas, sus mecánicas y los envases).
- Implementadas 3 animaciones de interfaz (album, tienda y recetario).
- Implementación de la tienda de temática gacha, así como de los 2 tipos de sobres y tienda de monedas premium.
- Diseño e implemenación de cartas funcionales de distintas rarezas.
- Implementación del álbum de cromos.
- Arreglados bugs y errores estéticos de programación.
- Arte final del diseño de estación de comida.
- Modelado y concept interior final.
- Modelos 3D (maquina de gacha, cafetera y elementos del escenario)
- Diseños 2D (monedas de pago y monedas normales)
- Diagrama de estados actualizado.
- Implementación de una fuente de texto personalizada.
- Cursores modificados (taza con cafe,agua, etc).
- Implementada la funcionalidad de la carta "*Latte Art*".
- Implementado el guardado de datos por partidas, creando la posibilidad de tener 3 partidas comenzadas y la opción de eliminar la que se desee.
- Implementado el guardado de monedas de la tienda y colección de cartas vinculado al navegador en lugar de a la partida concreta.
- Creación e implementación de música propia.
- Implementación de sonidos en algunos botones.

## 1. Introducción

### 1.1	Descripción breve del concepto
Un juego de simulación de cocina con mecánicas de gestión del tiempo y gachapon donde, mezclando el mundo del 3D y el 2D, el jugador debe gestionar su propia cafetería de gatos. Piensa en un “*Overcooked!*” más relajado y enfocado en cada cliente, pero con ese componente de gestión de negocios y progresión típico de un “*Restaurant Tycoon*”, añadiendo ese toque coleccionable del gachapon de “*Genshin Impact*”.

### 1.2	Descripción breve de la historia y personajes
La historia se desarrolla en una ciudad moderna y llena de vida, donde la protagonista, Catherine, ex estudiante de veterinaria, decide darle un giro de 180º a su vida. Debido a la crisis de adopciones en el refugio de la zona, decide reformar una antigua cafetería con el objetivo de convertirla en una nueva casa para gatos necesitados. 

Deberá hacer frente a las adversidades que se le presenten con el objetivo de dar cobijo a la mayor cantidad posible de gatos, empezando por su fiel compañero Moca.

### 1.3	Propósito, público objetivo y plataformas
Este juego busca generar una sensación de alegría y calma en el jugador, aportada por la estética del videojuego, añadiendo también una sensación de presión controlada que viene dada por la gestión del tiempo a la hora de realizar las comandas.

En cuanto al público objetivo al que nos dirigimos, sería un rango amplio de hombres y mujeres, de entre 14 y 35 años. Principalmente hablamos de jugadores casuales a los que les gustan los juegos de gestión de tiempo y recursos con ese toque de gambling aportado por el gachapon y que buscan sesiones de juego cortas, perfectas para jugar en el transporte público o durante descansos.


## 2. Monetización

### 2.1	Tipo de modelo de monetización
El juego seguirá un modelo freemium. La descarga y el acceso serán completamente gratuitos, pero existirá un sistema de progresión y coleccionables que incentivará a los jugadores a invertir tanto tiempo de juego como dinero real.

El progreso se vincula a la gestión del cat café, donde el jugador recibe recompensas al completar pedidos. Además, el sistema de gacha y sobres será el principal eje de monetización, ofreciendo una sensación de colección y sorpresa.

### 2.2 Tablas de productos y precios
Existe una moneda principal, que son las monedas del café que se obtienen al atender clientes y completar comandas. El jugador recibirá una recompensa diaria al iniciar sesión que serán 10 monedas junto con 1 sobre básico.

La cantidad de monedas recibidas será intencionalmente baja en comparación con el coste de los sobres, para incentivar la acumulación o el uso de moneda premium.

Queremos incorporar un tipo de gacha, donde existen 2 tipos de sobres:

1. **Sobre básico:**
    - Obtienes uno gratuito por día.
    - Contiene gatos comunes y decoraciones básicas para el café.
    - Posibilidad baja de obtener ítems raros.
    - Coste en tienda: bajo, fácil de conseguir si acumulas monedas suficientes del juego.

2. **Sobre premium:**
    - Requiere monedas premium o mayor cantidad de monedas normales.
    - Garantiza al menos un gato o ítem de rareza media-alta.
    - Mayor probabilidad de obtener gatos únicos.
    - Disponible en packs con descuentos.

- **Moneda premium:**
    - Son adquiribles solo con dinero real.
    - Sirven para comprar sobres premium directamente, acelerar la apertura de sobres o desbloquear los gatos.

- **Precios de la moneda premium:**
    - Pack 1: 100 croquetas doradas = 0,99 €
    - Pack 2: 550 croquetas doradas = 4,99 €
    - Pack 3: 1200 croquetas doradas = 9,99 €
    - Pack 4: 2500 croquetas doradas = 19,99 €

- **Costes en tienda interna:**
    - Sobre básico: 70 monedas del café.
    - Sobre premium: 220 monedas del café o 120 croquetas doradas.

| Producto         | Tipo de moneda                           | Coste                                          | Para qué sirve                                                | Disponibilidad       |
|:----------------:|:-----------------------------------------:|:----------------------------------------------:|:--------------------------------------------------------------:|:--------------------:|
| Monedas del café | Gratis                                    | Recompensa por comandas y login                | Sirven para comprar sobres básicos y mejoras menores            | Siempre              |
| Sobre básico     | Moneda del café                           | 70 monedas                                     | 1 a 3 ítems básicos                                            | Gratis diario + tienda |
| Sobre premium    | Moneda premium o monedas del café          | 220 monedas del café o 120 croquetas doradas   | 1 a 3 ítems raros o exclusivos                                 | Tienda               |
| Moneda premium   | Dinero real                               | Pack 1: 0,99 € (100)  <br> Pack 2: 4,99 € (550)  <br> Pack 3: 9,99 € (1200)  <br> Pack 4: 19,99 € (2500) | Moneda premium para sobres, acelerar progresos y cosméticos | Tienda online        |


### 2.3 Misión mensual en colaboración con Starbucks

Como parte de la estrategia de engagement y fidelización de los jugadores, se propone la implementación de una misión trimestral en colaboración con Starbucks. Esta dinámica busca generar expectación entre los participantes y convertir cada edición en un evento esperado dentro de la comunidad. Fomentando a su vez la participación activa dentro del juego, así como la interacción con la marca en redes sociales.

Durante el periodo establecido, los jugadores deberán acumular la mayor cantidad de puntos posibles, manteniendo un alto nivel de satisfacción media en sus partidas. Al finalizar la misión, los participantes deberán compartir su logro en las historias de Intagram u otras redes sociales como Twitter o Tiktok, utilizando el hashtag oficial de la campaña #StarWithCatpuccino.

El tiempo de publicación comienza el 1 del mes correspondiente y acaba el plazo el 30/31 de cada mes, en el 1 del mes siguiente se recopilarán todas las participaciones, se realiza un sorteo y ambas empresas publican sus resultados por todas las redes sociales. Se seleccionará a los cinco jugadores con mejor puntuación, quienes recibirán un descuento exclusivo en Starbucks de un 20% de descuento en todos los cafes de la primera semana después del sorteo, como recompensa por su desempeño y participación.

Esta iniciativa no solo incentiva la competitividad y el compromiso dentro del juego, sino que también refuerza la presencia de Starbucks en el entorno digital, asociando su imagen a valores de comunidad, diversión y experiencia compartida. De este modo, el descuento incentiva a los jugadores a incrementar su consumo de productos, impulsados tanto por el atractivo de la oferta como por su disponibilidad limitada en el tiempo. Esta estrategia fomenta una mayor frecuencia de consumo, ya que los usuarios buscan aprovechar el beneficio antes de que expire, generando así un aumento en las visitas y en la fidelización hacia la marca.

## 3. Planificación y costes

### 3.1	El equipo humano

- **Garazi Blanco Jauregi:** Artista de objetos y elementos interactuables.
- **Alberto Lopez Garcia de Ceca:** Artista de interfaces.
- **Pablo García García:** Programador de nivel y personajes. 
- **Cristina González de Lope:** Programadora de nivel e interfaces.
- **Ángela Sánchez Díaz:** Artista 2D y 3D de escenario y personajes.
- **Laura de la Cruz Cañadas:** Encargada de la música/efectos sonoros.

### 3.2	Estimación temporal del desarrollo
A continuación se muestra la estimación general de fases del proyecto:

- **Fase Alfa** → 2-3 semanas →  Desarrollo del prototipo del videojuego, creación de redes sociales y porfolio y selección de monetización para el videojuego.
- **Fase Beta** → 1 mes y medio → Implementación de assets propios, base del método de monetización, mecánicas finales y corrección de bugs más importantes.
- **Fase Release** → 2 meses y medio → Versión final del videojuego.

Dentro de la fase actual (la fase beta), se han desglosado las tareas y asignado a cada una su tiempos estimado, junto a sus respectivos responsables:

![Planificacion 1](https://github.com/MangoStudioSA/Catpuccino/blob/cef01d2def3f46c0ef8bde4d7b0ace0628ab2a8c/PhotosGDD/Planificacion%201.png)
![Planificacion 2](https://github.com/MangoStudioSA/Catpuccino/blob/18780670bb9e7ab196550aeb57017209ed34464c/PhotosGDD/Planificacion2.png)


## 4. Tutorial, mecánicas elementos de Juego

### 4.1 Tutorial de los minijuegos
Los clientes entrarán a la cafetería y, cuando lleguen al mostrador aparecerá un botón flotante sobre su cabeza sobre el que clicar.
Tutorial del minijuego de preparación del café:

- Los requisitos del pedido se pueden comprobar desplegando una nota clicando sobre el botón superior derecho.
- Se puede comprobar la receta del café actual haciendo clic en el botón superior izquierdo (libro de recetas).
- Se hace clic en la taza o en el vaso y se coloca en la cafetera haciendo clic.
    - Si se coloca una taza, habrá que colocar un plato en la bandeja, sino, no se podrá mover la taza a dicha bandeja.
- Se hace clic sobre el botón de la molinillo y se mantiene presionado hasta la cantidad solicitada.
- Se hace clic sobre la palanca y se mantiene presionada hasta que el semicirculo que sale por pantalla se complete y la palanca baje visualmente.
- Se hace clic sobre el filtro y se mueve a la cafetera haciendo clic.
- Se comprueba si hay que interactuar con algún ingrediente (ya que muchos de ellos se ocultarán al echar el café).
- Se hace clic sobre el botón izquierdo superior para comenzar con el proceso de echar el café:
    - Aparecerá una aguja en el centro de la cafetera rotando de izquierda a derecha. 
    - Objetivo: clicar sobre el botón inferior izquierdo de la cafetera con precisión cuando la aguja se encuentre en el centro.

A continuación, si el cliente ha solicitado azúcar, hielo o un pedido para llevar se tendrá que realizar lo siguiente:

- Se hace clic sobre la azucarera y se clica sobre la taza una vez (si el cliente ha solicitado poco azúcar) o dos veces (si el cliente ha solicitado mucho azúcar).
- Se hace clic sobre la hielera y se clica sobre la taza.
- Se hace clic sobre la tapa y se clica sobre el vaso.

*Si en algún momento se desea comenzar desde 0, se puede hacer clic sobre la papelera.

A partir del día 2 se desbloquea la zona de repostería.
Tutorial del minijuego de preparación de la comida:

- Los requisitos del pedido se pueden comprobar desplegando una nota clicando sobre el botón superior derecho.
- Primero, se hace clic sobre el plato o sobre la bolsa de llevar y se coloca en la bandeja.
- Posteriormente, se clica sobre el tipo de comida solicitado y se mueve al hornillo:
    - Para hornearlo hay que hacer clic sobre el botón superior derecho.
    - Al pulsarlo, aparecerá una barra que cambiará de color a media que pasan los segundos.
    - Cuando la barra llege a la zona comprendida entre el 50% y 75% se podrá parar mediante clic en el botón inferior derecho.
    - Si se para antes de tiempo se quedará crudo, mientras que si se  deja más tiempo del debido se quemará. Ambas repercutirán de forma negativa a la puntuación del jugador.
- Una vez horneada, se clica sobre la comida y se mueve al plato o a la bolsa de llevar.

El pedido se entrega desde la pantalla de preparación de cafés pulsando el botón "*Entregar*".

Se  mostrará una reacción por parte del cliente en función de la puntuació obtenida del minijuego. Además, si el jugador ha obtenido entre 92 y 100 puntos, obtendrá una propina. Cuando acabe el día, a las 20:00pm, se tendrán que pagar las facturas correspondientes y, si el jugador no tiene suficiente dinero, el juego finalizará. Por el contrario, si el jugador ha ingresado el dinero suficiente, podrá avanzar al siguiente día.


### 4.2	Descripción detallada del concepto de juego
En Catpuccino, el jugador se encargará de trabajar en una cafetería de gatos. Para esto, es necesario atender a los clientes, preparar su pedido con la mejor precisión y menor tiempo posible.

El objetivo principal es obtener los ingresos necesarios para lograr pasar de día e ir desbloqueando nuevos ingredientes. A su vez, se debe tratar de obtener el mayor porcentaje de satisfacción posible. Dicha satisfacción se trata de la media de puntuaciones obtenidas a lo largo de los días que el jugador ha trabajado en la cafetería.

### 4.3	Descripción detallada de las mecánicas de juego
El gameplay principal del juego se divide en tres etapas:

- **Atender a los clientes** → Poco a poco, los clientes irán entrando al local y poniéndose en la cola (si tienen a otro cliente por delante). Al llegar al mostrador, esperarán a que el jugador les atienda. Al ser atendidos, aparecerá una caja de diálogo en la que el jugador puede visualizar los detalles del pedido y aceptarlo, pasando a la siguiente pantalla.

- **Preparar el pedido** → Para preparar el pedido del cliente, el jugador tendrá que moler el café (teniendo en cuenta cómo lo ha solicitado el cliente), y preparar el envase correcto antes de echarlo. Además, la complejidad de preparación aumentará de forma progresiva, desbloqueando nuevos ingredientes cada día. De esta forma, el jugador deberá tratar de recordar las recetas de los distintos cafés que haya disponibles dicho día, pese a que podrá comprobarlas accediendo al recetario.

- **Entregar el pedido** → Una vez entregado el pedido, el cliente indicará su satisfacción. Si el pedido entregado cuadra con lo que quería el cliente, estará satisfecho y el jugador recibirá una propina adicional. En el caso de que el pedido esté mal preparado, el cliente indicará su frustración, dejando menos dinero (y reduciendo el porcentaje de satisfacción de los clientes).

Al finalizar la jornada, es necesario pagar las facturas correspondientes para mantener en funcionamiento el local (alquiler del local, costes de producción, publicidad…). Si el jugador ha conseguido suficientes beneficios para cubrir estos gastos, se podrá pasar al siguiente día de trabajo (continúa la partida), pero si el dinero ganado no es suficiente, finaliza la partida.

### 4.4	Controles
Para mejorar la sencillez y accesibilidad del juego, todas las interacciones se realizan mediante interfaces gráficas, es decir, usando el click izquierdo del ratón para interactuar con los distintos botones u objetos del juego. 

Además, esta implementación está pensada para facilitar el trabajo a la hora de adaptar el juego para dispositivos móviles (solo es necesario cambiar los clicks por toques a la pantalla).

### 4.5	Niveles y misiones
Aunque en Catpuccino no haya niveles como tal, el juego se basa en jornadas/días de trabajo. Para poder pasar al siguiente día, se debe acumular un mínimo de dinero (puntos) durante el día, ya que al finalizar la jornada, el jugador deberá pagar ciertas facturas para cubrir los gastos de gestión del local.

Cada día/jornada, estos gastos mínimos irán aumentando para aumentar la dificultad del juego. También aumentarán otros factores como la cantidad de clientes o la complejidad de sus pedidos. Además, la rapidez del paso del tiempo irá decreciendo a lo largo de los días.

El objetivo (misión) del juego es lograr aguantar el máximo de días posible, es decir, la partida no tiene un final definido. No es posible pasar al siguiente día de trabajo sin tener suficiente dinero para cubrir las facturas, en este caso, se alcanzará la pantalla de fin de juego. Se puede repetir el día tantas veces como el jugador quiera, ya que, cuando la cafetería cierra y el jugador paga las facturas, los datos se guardan. 

### 4.6	Objetos y elementos
Durante la preparación de pedidos, el jugador deberá interactuar con varios objetos para llevar a cabo la preparación el café del cliente. Entre ellos se ecuentran:

- **Taza/vaso** → Taza para pedidos para tomar y vasos para pedidos para llevar. Según el pedido del cliente se deberá escoger el recipiente adecuado junto con su combinación, ya que si se trata de un pedido para tomar el jugador deberá colocar previamente un plato.

- **Molinillo de café** → Encargado de moler la cantidad de café que desea el cliente. Los cafés pueden ser cortos, medios o largos. El jugador deberá mantener el click izquierdo y llenar la barra de café hasta el punto correcto.

- **Prensa** → Sirve para terminar de preparar el café molido antes de meterlo en la cafetera interactuando con la palanca del molinillo.

- **Cafetera** → Realiza la preparación del café, echándolo sobre el envase posicionado en ella. Aparece una aguja con movimiento horizontal de izquiera a derecha para indicar la precisión del jugador.

- **Azucarero** → Se pueden añadir las cucharadas de azúcar deseadas, para darle el toque de dulzura que desee el cliente. Esto se hará haciendo clic en la azucarera y clicando sobre la taza una o dos veces.

- **Hielera** → Se pueden añadir hielos para regular la temperatura de bebidas frías. Esto se realizará mediante clic en la hielera y clicando sobre el recipiente.

- **Tapa de vaso** → Si se trata de un pedido para llevar, el jugador tendrá que finalizar el pedido poniéndole la tapa al vaso mediante clic. 

- **Brick de leche** → A partir del día dos, estará disponible para añadir poca o mucha cantidad en el café.

- **Espumador y jarra de leche** → A partir del día 3, se podrá calentar la jarra de leche para añadirla al café.

- **Leche condensada** → A partir del día 5, se podrá añadir al café.

- **Crema** → A partir del día 5, se podrá añadir al café.

- **Sirope de chocolate** → A partir del día 7, se podrá añadir al café.

- **Botella de whiskey** → A partir del día 7, se podrá añadir al café.

- **Plato o bolsa** → Plato para pedidos para tomar y bolsa para pedidos para llevar.

- **Galletas, cupcakes y bizcochos** → Además de pedir una bebida, los clientes podrán elegir un acompañamiento entre distintos tipos de:
    - **Galletas:** De mantequilla, con pepitas de chocolate o chocolate blanco.
    - **Cupcakes:** De pistacho, arándanos, cereza o dulce de leche.
    - **Bizcochos o tartas:** De red velvet, chocolate, mantequilla o de zanahoria.

- **Hornillo** → Su función es manejar los estados de la comida, haciendola pasar de cruda a horneada y, si no se mueve en el debido momento, de horneada a quemada.

Tipos de cafés desbloqueados según el día en el que se encuentre el jugador:

- **Día uno:** Cafés de tipo espresso, lungo y americano.
- **Día dos:** Cafés de tipo macchiato.
- **Día tres:** Cafés de tipo latte y cappuccino.
- **Día cinco:** Cafés de tipo bombón, frappé y vienés.
- **Día siete:** Cafés de tipo irish y mocca.

Tipos de comidas desbloqueadas según el día en el que se encuentre el jugador:

- **Día dos:** Bizcochos.
- **Día cuatro:** Galletas.
- **Día seis:** Cupcakes.

### 4.7 Elementos desbloqueables por obtención de cartas

Con la implementación de la tienda y los sobres de cartas, se han implementado nuevas mecánicas que solo estarán disponibles si se tiene la carta asociada a ellas.
Se encuentr*an implementadas o en proceso de implementación las siguientes mecánicas:

- **Dibujos "*latte art*" de cafés:** Se desbloquean cuando se obtiene la carta de rareza épica "*Latte Art*". Esta mecánica consta de la modificacion de los sprites de los cafés *macchiato, mocca, irish, latte* y *vienés*. Cuando se obtenga esta carta, al preparar cualquiera de los cafés mencionados el sprite del café contará con un dibujo estilo *latte art* propio de las cafeterías. Por último, otorgará un 20% más de propina cuando se haga uso de ellos en la preparación de dichos cafés.

- **Skins de vaso y de tapa de vaso:** Se desbloquean cuando se obtiene las cartas de rareza intermedia "*Skin Vaso*". Esta mecánica consta de la modificacion de los sprites de los vasos y sus tapas por otro más especial. Su uso otorgará un 10% más de propina cuando se haga uso de ellos en la preparación de los cafés.

- **Skins de taza y de plato:** Se desbloquean cuando se obtiene la carta de rareza legendaria "*Skin Taza*". Esta mecánica consta de la modificacion de los sprites de las tazas y sus platos a juego por otro más especial. Su uso otorgará un 25% más de propina cuando se haga uso de ellos en la preparación de los cafés.

## 5. Trasfondo

### 5.1	Descripción detallada de la historia y la trama
Catherine es una mujer de 21 años que es voluntaria en un refugio de animales. Aunque su plan principal era ser veterinaria, después de ver las pocas adopciones que tienen en el refugio, en su tercer año de carrera decide dejarlo todo y abrir una cafetería donde poder brindar un hogar a los gatos del refugio.

Al principio es duro y solo puede permitirse adoptar a un gato, al que llama Moka, pero poco a poco, según va ganando clientela y expandiendo su negocio, va adoptando más y más gatos.

### 5.2	Personajes
**Catherine**: Es una mujer de 21 años dueña de una cafetería. Es de estatura media y complexión delgada, ojos verdes y el pelo largo y castaño recogido en una coleta. Es el personaje principal y su objetivo es dar un hogar a los gatos que no lo tienen.

**Moca**: Es el primer gato que adopta Catherine. Es un gato tranquilo y obediente, de color cafe y ojos amarillos. Siempre está en la barra de la cafetería haciendo compañía a su dueña.

### 5.3	Entornos y lugares
El lugar principal del juego es la cafetería de Catherine, la que cuenta con un único espacio abierto separado por una barra que delimita por donde se pueden mover camarera y clientes. Hay varias mesas para que los clientes se puedan sentar y taburetes en la barra para los que prefieran sentarse en ella. En la decoración predominan los colores pastel y tiene un estilo visual delicado, que refuerza la identidad acogedora y distintiva del lugar. 


## 6. Arte

### 6.1	Estética general del juego
El juego busca transmitir una atmósfera acogedora y adorable, típica de los juegos cozy. La cafetería es donde se desarrolla toda la acción del juego, por lo que esta tendrá que tener un diseño que transmite un aire hogareño y relajante.

El estilo artístico que sigue juego, tanto en el 2D como en el 3D, será un estilo cartoon redondeado. En el caso del 2D tendrá una línea semi fina y sombras simples, para imitar al 3D.

En la paleta de colores predominarán los colores pastel, como rosas, marrones, cremas y verdes suaves, que transmiten la calma y dulzura que el juego emite.

La inspiración para la parte 3D la encontramos en juegos como Animal Crossing, Ooblets o Slime Rancher y para el 2D juegos como Buena pizza Gran pizza, Bear and Breakfast o Night in the Woods.

### 6.2	Apartado visual
El estilo que tendrán los personajes es un estilo semi-chibi, con cabezas un poco más grandes para acentuar expresiones. La vestimenta que llevarán es casual y algo coqueto, pero depende de la personalidad de cada cliente. Las animaciones serán simples pero expresivas, indicando con estas si el cliente está satisfecho con tu trabajo o no.

El escenario tendrá una estética cálida con maderas, plantas y luces colgantes. También dispondrá de zonas decorables por el jugador, como mesas, sillas y la decoración de las paredes… .
Los props serán las tazas, los pasteles, la máquina de café, los gatos… . Los elementos decorativos son cuadros, cojines, lámparas, estanterías con libros y teteras. Estos elementos tendrán pequeñas animaciones como vapor saliendo de las tazas o  billos si está el pedido está hecho perfecto.

La interfaz se explicará más detalladamente en el siguiente apartado, pero esta tendrá botones redondeados, colores pastel, tipografía manuscrita o cursiva redonda. Contará con una iconografía clara como taza para café, la cara de un gato para los gatos, moneda para economía. El HUD contará con un diseño minimalista que muestre elementos como el dinero, la satisfacción de tus clientes y el estado de los gatos.

### 6.3	Referencias
Las referencias principales para el 2D son *Buena Pizza, Gran Pizza* y *Little Corner Tea House* y para el 3D *Animal Crossing* y *Slime Rancher*.

| ![Buena Pizza, Gran Pizza](https://github.com/MangoStudioSA/Catpuccino/blob/6fd13b888c0d7e09d381f4ada87c7a1560359df1/PhotosGDD/BuenaPizza.jpg) | ![Little Corner Tea House](https://github.com/MangoStudioSA/Catpuccino/blob/6fd13b888c0d7e09d381f4ada87c7a1560359df1/PhotosGDD/LittleCornerTeaHouse.jpg) |
|-------------------------------|-------------------------------|
| ![Animal Crossing](https://github.com/MangoStudioSA/Catpuccino/blob/6fd13b888c0d7e09d381f4ada87c7a1560359df1/PhotosGDD/AnimalCrossing.jpg) | ![Slime Rancher](https://github.com/MangoStudioSA/Catpuccino/blob/6fd13b888c0d7e09d381f4ada87c7a1560359df1/PhotosGDD/SlimeRancher.jpeg) |


### 6.4	Logo de la empresa
![Logo Mango Studio](https://github.com/MangoStudioSA/Catpuccino/blob/e921cc81d0e6f43bd58ed52fc449a0f76f782ea9/PhotosGDD/LogoMangoStudio.PNG)

### 6.5	Logo del videojuego
![Logo Catpuccino](https://github.com/MangoStudioSA/Catpuccino/blob/e921cc81d0e6f43bd58ed52fc449a0f76f782ea9/PhotosGDD/LogoCatpuccino.PNG)

### 6.6	Concept Art personaje principal
![Concept Art PJ Principal](https://github.com/MangoStudioSA/Catpuccino/blob/e921cc81d0e6f43bd58ed52fc449a0f76f782ea9/PhotosGDD/conceptPJPrincipal.png)
![Arte final PJ Principal](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/EXPRESIONES.png)

### 6.7 Props
#### 6.7.1 Cafes
Se han relizado todas las variaciones necesarias para los props de los cafes, tanto en las tazas (para tomar aqui) como el los vasos (para llevar), ademas, se han dibujado tambien etos mismos cafes con todas las variaciones de skins existentes.

![CAFES](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/CAFES.png)

#### 6.7.2 Postres
Al igual que con los cafes, para los postres se han dibujado todas las galletas, muffins y tartas necesarias junto con todas sus variaciones.

![TARTAS](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/TARTAS.png)
![MUFFINSYGALLETAS](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/MUFFINSYGALLETAS.png)

#### 6.7.3 Estacion de cafetera
Todos los objetos que se pueden observar en la siguiente imagen estan hechos por separado, esto incluye botones y otros objetos funcionales que requieren varios sprites.

![REFERENCIAEstacionCafetera](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/REFERENCIAEstacionCafetera.png)

#### 6.7.4 Estacion de Postres
Todos los objetos que se pueden observar en la siguiente imagen estan hechos por separado, esto incluye botones y otros objetos funcionales que requieren varios sprites.

![REFERENCIAEstacionComida](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/REFERENCIAEstacionComida.png)

### 6.8 Cartas y sobres de cartas

![Cartas y sobres de cartas](https://github.com/MangoStudioSA/Catpuccino/blob/94eed32a71679b0de7f116218af42cba7f10aa56/PhotosGDD/cartas.png)

### 6.8 Clientes
Para los clientes, se han realizado 3 ilustraciones para la pantalla de pedido, una con una expresion neutra, una con una expresion enfadada o triste y otra con una expresion alegre. Ademas, se ha realizado un modelado 3d sencillo de cada cliente.

#### 6.8.1 Cliente 1 "chicaEmo"

![chicaEmo](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/chicaEmoREFERENCIA.png)

#### 6.8.2 Cliente 2 "chicaCoqueta"

![chicaCoqueta](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/chicaCoquetaREFERENCIA.png)

#### 6.8.3 Cliente 3 "Gracie"

![Gracie](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/GracieFinal.png)

#### 6.8.4 Cliente 4 "Mindie"

![Mindie](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/MindieFinal.png)


## 7. Sonido
El estilo musical será lo-fi chillhop con influencias de jazz suave. El objetivo de utilizar este estilo de música es reforzar la sensación de estar en un lugar cómodo y sin estrés. Los instrumentos que deberá utilizar la música serán piano, contrabajo suave, percusión ligera, guitarra acústica, toques de sintetizador lo-fi.

Para cada sección del juego se tendrá temas musicales diferentes:

- **Menú principal:** melodia relajante con piano y guitarra acústica.
- **Juego:** loops de música tranquila, donde a veces se podrá escuchar distintos barks como maullidos y la estática de vinilos.
- **Horas pico:** la música será un poco más dinámica, para crear un poco de tensión al jugador.

### 7.1	Ambiente sonoro
El ambiente sonoro del juego busca recrear la calidez de una cafetería, transmitiendo la sensación de un espacio tranquilo y agradable. Se escucharán sonidos característicos de las cafeterías como el goteo de un café al prepararse, el vapor de las máquinas y la campanita de los clientes entrando. Además se escucharan los maullidos de los gatos, el murmullo de los clientes teniendo conversaciones y el arrastre suave de sillas, creando así una atmósfera relajada. A lo largo del día, el ambiente sonoro cambiará sutilmente, en las horas pico el sonido de las diferentes actividades de los clientes sonará más alto, mientras que en los momentos de calma se podrá escuchar más a los gatos. Todos estos elementos sonoros estarán mezclados de manera estilizada, no buscando un realismo extremo, si no crear un ambiente atmosférico de calma y suavidad, siguiendo la estética general del juego.


## 8. Interfaz

### 8.1 Diseños básicos de los menús
En cuanto al estilo artístico y principios generales de la interfaz, esta tendrá una estética cartoon siguiendo el estilo de toda la parte 2D del videojuego. A su vez la paleta de colores serán pasteles, algunos de ellos con textura marmolada aportándole interés y profundidad a la interfaz. En cuanto a los fonts de la interfaz se contaran con varios, siendo el principal uno efecto tiza para botones y demas, y otro estilo arial para los bocadillos de texto de los clientes. La interfaz permitira cambiar tanto el font de la interfaz por uno mas simple, con el objetivo de hacer una interfaz amigable para todos los usuarios, asi como el cambio del idioma de esta, teniendo como posibilidades: español, ingles y euskera.

Las interfaces contarán con animaciones al clickear botones o interactuar con ellas con el objetivo de aportar dinamismo al juego a la vez que se le da vida a la propia interfaz.

A continuación se muestran imágenes de la base para el diseño de cada una de las pantallas de la interfaz para conocer más en detalle cuales son los assets necesarios para el desarrollo de dicha interfaz, así como algunos de los botones que ya han sido diseñados para estas.

### Interfaces
| ![MainMenu](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/MainMenu.png) | ![Opciones](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/Opciones.png) |
|-------------------------------|-------------------------------|
| ![Contacto](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/Contacto.png) | ![Ingame](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/Ingame.png) |
| ![Pausa](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/Pausa.png) | ![Pedido](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/Pedido.png) |
| ![Preparacion](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/Preparacion1.0.png) | ![PanelEntrega](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/PanelEntrega.png) |
| ![FinDia](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/FinDia.png) | ![FinJuego](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/FinJuego.png) |

### Botones
![BotonJugar](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/botonJugar.png)
| ![botonMenuInicio](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/botonMenuInicio.png) | ![botonMenuInicio2](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/botonMenuInicio2.png) |
|-------------------------------|-------------------------------|
| ![botonPausa](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/botonPausa.png) | ![botonAjustes](https://github.com/MangoStudioSA/Catpuccino/blob/main/PhotosGDD/botonAjustes.png) |


### 8.2 Diagrama de flujo
![Diagrama de flujo beta](https://github.com/MangoStudioSA/Catpuccino/blob/59c751fd00869fc76fd09a048ff4afaea7bff875/PhotosGDD/Diagrama%20de%20flujo%20beta.png)

## 9. Hoja de ruta del desarrollo

| #Hito         | Descripción                                  | Fecha                             | 
|:-------------:|:--------------------------------------------:|:---------------------------------:|
| 1             | GDD - 1 Hoja                                 | 12/10/2025                        |
| 2             | Sistema de facturas/día                      | 16/10/2025                        |
| 3             | Concept art cartas                           | 05/11/2025                        |
| 4             | Concept art personajes                       | 13/11/2025                        |
| 5             | Concept art props                            | 13/11/2025                        |
| 6             | Concept art escenario                        | 13/11/2025                        |
| 7             | Concept art interfaces                       | 13/11/2025                        |
| 8             | Sistema de cartas gacha                      | 14/11/2025                        |
| 9             | Implementación empleados                     | 14/11/2025                        |
| 10            | Implementación de clientes inteligentes      | 25/11/2025                        |
| 11            | Mecánicas completas                          | 01/12/2025                        |
| 12            | Pulido completo                              | 11/12/2025                        |
| 13            | GDD - Completo                               | 11/12/2025                        |

- **Fecha de lanzamiento**: 12 de diciembre de 2025
