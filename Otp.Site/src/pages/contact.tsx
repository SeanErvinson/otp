import Layout from "@theme/Layout";
import React from "react";

const Contact = () => {
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
      <form name="Contact" method="POST" data-netlify="true">
        <p>
          <label>
            Your Name: <input type="text" name="name" />
          </label>
        </p>
        <p>
          <label>
            Your Email: <input type="email" name="email" />
          </label>
        </p>
        <p>
          <label>
            Your Role:{" "}
            <select name="role[]" multiple>
              <option value="leader">Leader</option>
              <option value="follower">Follower</option>
            </select>
          </label>
        </p>
        <p>
          <label>
            Message: <textarea name="message"></textarea>
          </label>
        </p>
        <p>
          <button type="submit">Send</button>
        </p>
      </form>
      <hr />
      <form name="newsletter" method="POST" data-netlify="true">
        <p>
          <label>
            Email <input type="email" name="email" />
          </label>
        </p>
        <p>
          <button type="submit">Send</button>
        </p>
      </form>
    </Layout>
  );
};

export default Contact;
