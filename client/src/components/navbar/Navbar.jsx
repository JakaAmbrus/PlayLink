import React, { useState } from 'react';
import './Navbar.scss';
import PlayCubeLogoWhite from './img/logo-white.png';
import PlayCubeLogoGrey from './img/logo-grey.png';

export default function App() {
  const [isHovered, setIsHovered] = useState(false);

  const handleContainerHover = () => {
    setIsHovered(true); 
  };

  const handleContainerLeave = () => {
    setIsHovered(false);
  };

  return (
    <nav id='navigation-bar'>
      <div id="navbar-left"
      onMouseEnter={handleContainerHover}
      onMouseLeave={handleContainerLeave}>
        <div id="logo-container">
          <img id='logo' className={isHovered ? 'animate-logo' : ''} src={PlayCubeLogoWhite} alt="PlayLink logo" />
        </div>
        <div id='logo-text-container'>
          <div id='logo-play-text-container'><div id="logo-text-play" className={isHovered ? 'animate-play' : ''}>Play</div></div>
          <div id='logo-link-text-container'><div id='logo-text-link' className={isHovered ? 'animate-link' : ''}>Link</div></div>
        </div>
      </div>
      <div id="navbar-right"></div>
    </nav>
  );
}
