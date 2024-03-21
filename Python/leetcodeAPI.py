import requests
import json
import requests
import json
import random
import os

def get_problem_slugs(slugs_filename):
    problem_slugs = []
    if os.path.exists(slugs_filename):
        with open(slugs_filename, 'r')as file:
            problem_slugs = [line.strip() for line in file.readlines()]
        return problem_slugs
    url = "https://leetcode.com/graphql"
    query = """
    {
      allQuestions {
        titleSlug
      }
    }
    """
    response = requests.post(url, json={'query': query})
    if response.status_code == 200:
        data = response.json()
        problem_slugs = [problem['titleSlug'] for problem in data['data']['allQuestions']]
        with open(slugs_filename, 'w') as file:
            for slug in problem_slugs:
                file.write(slug +'\n')
        return problem_slugs
    else:
        print('error retrieving problems')
        return []
def get_problem_description(title_slug):
    url = "https://leetcode.com/graphql"
    query = """
    query getProblemDetails($titleSlug: String!) {
      question(titleSlug: $titleSlug) {
        questionId
        title
        difficulty
        content
        hints
      }
    }
    """
    variables = {
        "titleSlug": title_slug
    }
    response = requests.post(url, json={'query': query, 'variables': variables})
    if response.status_code == 200:
        try:
            data = response.json()
            question = data.get('data', {}).get('question', {})
            return {
                "questionId": question.get('questionId', None),
                "title": question.get('title', None),
                "difficulty": question.get('difficulty', None),
                "content": question.get('content', None),
                "hints": question.get('hints', None)
            }
        except Exception as e:
            print(f"An error occurred: {e}")
            print(response.text)
            return None
    else:
        return "Error fetching problem description"

if __name__ == "__main__":
    title_slug = "two-sum"  # Example titleSlug
    problem_details = get_problem_description(title_slug)
    has_none = any(value is None for value in problem_details.values())
    print(has_none)
    print(problem_details)
    print("This print statement is from leetcodeAPI.py")
