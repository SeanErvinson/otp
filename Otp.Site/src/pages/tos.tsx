import Layout from "@theme/Layout";
import React, { FormEvent } from "react";

const Contact = () => {
  const handleOnSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    console.log(event.currentTarget);
    fetch("/contact", {
      method: "POST",
      headers: { "Content-Type": "application/x-www-form-urlencoded" },
      body: new URLSearchParams(
        new FormData(event.currentTarget) as any
      ).toString(),
    })
      .then(() => console.log("Form successfully submitted"))
      .catch((error) => alert(error));
  };

  return (
    <Layout title="Contact Us" description="Very simple and affordable">
      <div className="container px-6 py-16 mx-auto text-center">
        <div className="max-w-xl mx-auto">
          <h1 className="text-3xl font-bold text-gray-800 dark:text-white md:text-4xl">
            Contact Us
          </h1>
          <p className="mt-6 text-lg text-gray-500 dark:text-gray-300">
            Have any questions or suggestions? We'd love to hear from you.
          </p>
        </div>
      </div>
      <form name="newsletter" data-netlify="true" onSubmit={handleOnSubmit}>
        <input type="hidden" name="form-name" value="newsletter" />
        <p>
          <label>
            Email <input type="email" name="email" />
          </label>
        </p>
      </form>
    </Layout>
  );
};

export default Contact;
