import React from "react";

const Footer = () => {
  return (
    <div className=" mt-auto footer align-self-center">
      <p className="mb-1">
        Copyright © 2023 Dragana Banovic, Software Developer and Computer
        Science Teacher
      </p>
      <p className="mb-1">
        Find me on:
        <a
          href="https://www.linkedin.com/in/dragana-banovi%C4%87-52b9ba257/"
          target="_blank"
          className="mx-1"
        >
          <img
            className="img-fluid"
            src="/images/linkedin.png"
            width={15}
            alt="linkedin"
          />
        </a>
        <a
          href="https://github.com/draganabanovic"
          target="_blank"
          className="mx-1"
        >
          <img
            className="img-fluid"
            src="/images/github.png"
            width={15}
            alt="github"
          />
        </a>
      </p>
    </div>
  );
};

export default Footer;
