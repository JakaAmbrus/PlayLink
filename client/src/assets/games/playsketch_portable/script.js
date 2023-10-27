
const entireGame = document.getElementById('entire-game');
const fullContainer = document.getElementById('full-container');
const startScreen = document.getElementById('start-screen');
const toggleOptions = document.getElementById('toggle-options');
const gameScreen = document.getElementById('game-screen');
const startTxt = document.getElementById('press-to-start');
const optionsContainer = document.getElementById('options-container');
const techContainer = document.getElementById('tech-container');
const rotate = document.getElementById('rotate');
//buttons
const optionsBtn = document.getElementById("options-btn");
const techBtn = document.getElementById("tech-btn");
const upBtn = document.getElementById("up-btn");
const downBtn = document.getElementById("down-btn");
const leftBtn = document.getElementById("left-btn");
const rightBtn = document.getElementById("right-btn");
const xBtn = document.getElementById("x-btn");
const sBtn = document.getElementById("s-btn");
const tBtn = document.getElementById("t-btn");
const oBtn = document.getElementById("o-btn");
const colorBtn = document.getElementById("color-btn");
const fullscreenBtn = document.getElementById("fullscreen-btn");
const rainbowBtn = document.getElementById("rainbow-btn");
const muteBtn = document.getElementById("mute-btn");
const unmuteBtn = document.getElementById("unmute-btn");
const clearBtn = document.getElementById("clear-btn");
const startBtn = document.getElementById("start-btn");
const beep = document.getElementById("beep");
const startAudio = document.getElementById("start-audio");
let gameAudio = document.querySelectorAll('.my-audio');
const clear = document.getElementById("clear");

//start screen
startBtn.addEventListener('click', startGameFunction);

function startGameFunction()
{
    toggleOptions.style.opacity = '1';
    startBtn.style.pointerEvents = 'none';
    startBtn.style.cursor = 'none';
    startTxt.style.animation = 'none';
    startTxt.style.opacity = '0';
    startAudio.play();
    setTimeout(() => {
        startScreen.style.display = 'none';
        gameScreen.style.display = 'grid';
        
    }, 4000)
}

//options and tech
techBtn.addEventListener('click', function() {
    overlay(techContainer);
    beep.play();
  });
optionsBtn.addEventListener('click', function() {
    overlay(optionsContainer);
    beep.play();
  });
function overlay(n)
{
    if (n.style.opacity === '1') {
        n.style.opacity = '0';
        n.style.display = 'none';
      } else {
        n.style.opacity = '1';
        n.style.display = 'block';
      }
}

//the game
let color = 'black';
let isDrawing = false;

function generateDivs(columns, rows) {
  gameScreen.innerHTML = '';

  for (let i = 0; i < columns * rows; i++) {
    let box = document.createElement('div');
    box.style.border = '1px solid gray';
    box.addEventListener('mousedown', startDrawing);
    box.addEventListener('mouseup', stopDrawing);
    box.addEventListener('touchstart', startDrawing);
    box.addEventListener('touchend', stopDrawing);
    box.addEventListener('touchcancel', stopDrawing);
    box.addEventListener('touchmove', draw);
    box.addEventListener('mousemove', draw);
    gameScreen.style.gridTemplateRows = `repeat(${rows}, 1fr)`;
    gameScreen.style.gridTemplateColumns = `repeat(${columns}, 1fr)`;
    gameScreen.appendChild(box).classList.add('section');
  }
}

let x = 1;
generateDivs(23, 14);

function startDrawing(event) {
  event.preventDefault();
  isDrawing = true;
  draw(event);
}

function stopDrawing() {
  isDrawing = false;
}

function draw(event) {
  if (isDrawing) {
    const square = event.target;
    if (square && square.classList.contains('section')) {
      if (color === 'random') {
        square.style.backgroundColor = `hsl(${Math.random() * 360}, 100%, 50%)`;
      } else {
        square.style.backgroundColor = color;
      }
    }
  }
}

const colorPicker = document.getElementById("color-picker");
colorPicker.addEventListener("change", updateColor);

function showColorPicker() {
  beep.play();
  colorPicker.click();
}

function updateColor(event) {
  const selectedColor = event.target.value;
  changeColor(selectedColor);
}

function changeColor(choice) {
  color = choice;
  beep.play();
}


clearBtn.addEventListener('click', clearBoard);

function clearBoard() {
  const sections = document.getElementsByClassName('section');
  clear.play();
  for (let i = 0; i < sections.length; i++) {
    sections[i].style.backgroundColor = '';
  }
}
upBtn.addEventListener('click', increaseDivs);
downBtn.addEventListener('click', decreaseDivs);

function increaseDivs() {
    clearBoard()
    if(x == 1){
        generateDivs(46, 29)
        x = 2;
    }
    else if(x == 0){
        generateDivs(23, 14)
        x = 1
    }
    else if(x == 2){
        generateDivs(55, 35)
        x = 3
    }
    
}
function decreaseDivs() {
    clearBoard()
    if(x == 1){
        generateDivs(18, 12)
        x = 0;
    }
    else if(x == 2){
        generateDivs(23, 14)
        x=1
    }
    else if(x == 3){
        generateDivs(46, 29)
        x=2

    }
    
}
//mute buttons
muteBtn.addEventListener('click', function() {
    muteBtn.style.pointerEvents = 'none';
    muteBtn.style.cursor = 'none';
    unmuteBtn.style.pointerEvents = 'auto';
    unmuteBtn.style.cursor = 'pointer';
    gameAudio.forEach(function(audio) {
        audio.muted = true; 
      });
  });
  
  unmuteBtn.addEventListener('click', function() {
    beep.play();
    unmuteBtn.style.pointerEvents = 'none';
    unmuteBtn.style.cursor = 'none';
    muteBtn.style.pointerEvents = 'auto';
    muteBtn.style.cursor = 'pointer';
    gameAudio.forEach(function(audio) {
        audio.muted = false; 
      });
  });

  //fulcrenn button
  fullscreenBtn.addEventListener('click', function() {
    if (!document.fullscreenElement && !document.webkitFullscreenElement && !document.mozFullScreenElement && !document.msFullscreenElement) {
      // Enter fullscreen
      if(window.matchMedia('(max-width: 1100px)').matches || window.matchMedia('(min-height: 2500px)').matches){
        rotate.style.display = 'flex';
        setTimeout(() => {
          rotate.style.opacity = '1';
        }, 100)
        setTimeout(() => {
          rotate.style.display = 'none';
          rotate.style.opacity = '0';
        }, 2000)
      }
      if (entireGame.requestFullscreen) {
        entireGame.requestFullscreen();
      } else if (entireGame.mozRequestFullScreen) {
        entireGame.mozRequestFullScreen();
      } else if (entireGame.webkitRequestFullscreen) {
        entireGame.webkitRequestFullscreen();
      } else if (entireGame.msRequestFullscreen) {
        entireGame.msRequestFullscreen();
      }
    } else {
      // Exit fullscreen
      if (document.exitFullscreen) {
        document.exitFullscreen();
      } else if (document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
      } else if (document.webkitExitFullscreen) {
        document.webkitExitFullscreen();
      } else if (document.msExitFullscreen) {
        document.msExitFullscreen();
      }
    }
  });
  
