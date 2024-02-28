from pymongo import MongoClient
import random
import leetcodeAPI

DEBUG = True
connection_string = "mongodb+srv://root:root@interview-frontend.0nwuz0e.mongodb.net/"
client = MongoClient(connection_string)


slugs_filename = 'leetcode_problem_slugs.txt'
problem_list = leetcodeAPI.get_problem_slugs(slugs_filename)

def write_random_leetcode_problems(problem_list, count = 5):
    skipped_problems = 0
    db = client['LeetcodeData']
    collection = db['problems_test']
    if not DEBUG:
        collection = db['Problems']
    collection.delete_many({})
    random.shuffle(problem_list)
    assert(count <= len(problem_list))
    for i in range(count):
        problem = problem_list[i]
        problem_details = leetcodeAPI.get_problem_description(problem)
        if problem_details is None or any(value is None for value in problem_details.values()):
            skipped_problems += 1
            continue
        try:
            collection.insert_one(problem_details)
        except Exception as e:
            print(f"An error occurred: {e}")
    print(skipped_problems)

def write_all_leetcode_problems(problem_list):
    write_random_leetcode_problems(problem_list,count = len(problem_list))
    
if __name__ == "__main__":
    write_all_leetcode_problems(problem_list)
